// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Security.Cryptography;
using Aspire.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;

namespace CraftersCloud.ReferenceArchitecture.AppHost.Tests.Infrastructure;

// extensions used from https://github.com/dotnet/aspire-samples/blob/main/tests/SamplesIntegrationTests/SamplesIntegrationTests.csproj
public static partial class DistributedApplicationExtensions
{
    /// <summary>
    /// Ensures all parameters in the application configuration have values set.
    /// </summary>
    public static TBuilder WithRandomParameterValues<TBuilder>(this TBuilder builder)
        where TBuilder : IDistributedApplicationTestingBuilder
    {
        var parameters = builder.Resources.OfType<ParameterResource>().Where(p => !p.IsConnectionString).ToList();
        foreach (var parameter in parameters)
        {
            builder.Configuration[$"Parameters:{parameter.Name}"] = parameter.Secret
                ? PasswordGenerator.Generate(16, true, true, true, false, 1, 1, 1, 0)
                : Convert.ToHexString(RandomNumberGenerator.GetBytes(4));
        }

        return builder;
    }

    /// <summary>
    /// Sets the container lifetime for all container resources in the application.
    /// </summary>
    public static TBuilder WithContainersLifetime<TBuilder>(this TBuilder builder, ContainerLifetime containerLifetime)
        where TBuilder : IDistributedApplicationTestingBuilder
    {
        var containerLifetimeAnnotations = builder.Resources.SelectMany(r => r.Annotations
                .OfType<ContainerLifetimeAnnotation>()
                .Where(c => c.Lifetime != containerLifetime))
            .ToList();

        foreach (var annotation in containerLifetimeAnnotations)
        {
            annotation.Lifetime = containerLifetime;
        }

        return builder;
    }

    /// <summary>
    /// Replaces all named volumes with anonymous volumes so they're isolated across test runs and from the volume the app uses during development.
    /// </summary>
    /// <remarks>
    /// Note that if multiple resources share a volume, the volume will instead be given a random name so that it's still shared across those resources in the test run.
    /// </remarks>
    public static TBuilder WithRandomVolumeNames<TBuilder>(this TBuilder builder)
        where TBuilder : IDistributedApplicationTestingBuilder
    {
        // Named volumes that aren't shared across resources should be replaced with anonymous volumes.
        // Named volumes shared by mulitple resources need to have their name randomized but kept shared across those resources.

        // Find all shared volumes and make a map of their original name to a new randomized name
        var allResourceNamedVolumes = builder.Resources.SelectMany(r => r.Annotations
                .OfType<ContainerMountAnnotation>()
                .Where(m => m.Type == ContainerMountType.Volume && !string.IsNullOrEmpty(m.Source))
                .Select(m => (Resource: r, Volume: m)))
            .ToList();
        var seenVolumes = new HashSet<string>();
        var renamedVolumes = new Dictionary<string, string>();
        foreach (var resourceVolume in allResourceNamedVolumes)
        {
            var name = resourceVolume.Volume.Source!;
            if (!seenVolumes.Add(name) && !renamedVolumes.ContainsKey(name))
            {
                renamedVolumes[name] = $"{name}-{Convert.ToHexString(RandomNumberGenerator.GetBytes(4))}";
            }
        }

        // Replace all named volumes with randomly named or anonymous volumes
        foreach (var (resource, volume) in allResourceNamedVolumes)
        {
            var newName = renamedVolumes.GetValueOrDefault(volume.Source!);
            var newMount =
                new ContainerMountAnnotation(newName, volume.Target, ContainerMountType.Volume, volume.IsReadOnly);
            resource.Annotations.Remove(volume);
            resource.Annotations.Add(newMount);
        }

        return builder;
    }

    /// <summary>
    /// Waits for the specified resource to reach the specified state.
    /// </summary>
    public static Task WaitForResource(this DistributedApplication app, string resourceName, string? targetState = null,
        CancellationToken cancellationToken = default)
    {
        targetState ??= KnownResourceStates.Running;
        var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();

        return resourceNotificationService.WaitForResourceAsync(resourceName, targetState, cancellationToken);
    }

    /// <summary>
    /// Waits for all resources in the application to reach one of the specified states.
    /// </summary>
    /// <remarks>
    /// If <paramref name="targetStates"/> is null, the default states are <see cref="KnownResourceStates.Running"/> and <see cref="KnownResourceStates.Hidden"/>.
    /// </remarks>
    public static async Task WaitForResourcesAsync(this DistributedApplication app,
        IEnumerable<string>? targetStates = null, CancellationToken cancellationToken = default)
    {
        var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(WaitForResourcesAsync));

        targetStates ??=
            [KnownResourceStates.Running, KnownResourceStates.Hidden, ..KnownResourceStates.TerminalStates];
        targetStates = targetStates.ToArray();
        var applicationModel = app.Services.GetRequiredService<DistributedApplicationModel>();
        var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();

        var resourceTasks = new Dictionary<string, Task<(string Name, string State)>>();

        foreach (var resource in applicationModel.Resources)
        {
            resourceTasks[resource.Name] = GetResourceWaitTask(resource.Name, targetStates, cancellationToken);
        }

        logger.LogInformation("Waiting for resources [{Resources}] to reach one of target states [{TargetStates}].",
            string.Join(',', resourceTasks.Keys),
            string.Join(',', targetStates));

        while (resourceTasks.Count > 0)
        {
            var completedTask = await Task.WhenAny(resourceTasks.Values);
            var (completedResourceName, targetStateReached) = await completedTask;

            if (targetStateReached == KnownResourceStates.FailedToStart)
            {
                throw new DistributedApplicationException($"Resource '{completedResourceName}' failed to start.");
            }

            resourceTasks.Remove(completedResourceName);

            logger.LogInformation("Wait for resource '{ResourceName}' completed with state '{ResourceState}'",
                completedResourceName, targetStateReached);

            // Ensure resources being waited on still exist
            var remainingResources = resourceTasks.Keys.ToList();
            for (var i = remainingResources.Count - 1; i > 0; i--)
            {
                var name = remainingResources[i];
                if (applicationModel.Resources.All(r => r.Name != name))
                {
                    logger.LogInformation("Resource '{ResourceName}' was deleted while waiting for it.", name);
                    resourceTasks.Remove(name);
                    remainingResources.RemoveAt(i);
                }
            }

            if (resourceTasks.Count > 0)
            {
                logger.LogInformation(
                    "Still waiting for resources [{Resources}] to reach one of target states [{TargetStates}].",
                    string.Join(',', remainingResources),
                    string.Join(',', targetStates));
            }
        }

        logger.LogInformation("Wait for all resources completed successfully!");
        return;

        async Task<(string Name, string State)> GetResourceWaitTask(string resourceName,
            IEnumerable<string> targetStates1, CancellationToken ct)
        {
            var state = await resourceNotificationService.WaitForResourceAsync(resourceName, targetStates1,
                ct);
            return (resourceName, state);
        }
    }

    /// <summary>
    /// Gets the app host and resource logs from the application.
    /// </summary>
    public static (IReadOnlyList<FakeLogRecord> AppHostLogs, IReadOnlyList<FakeLogRecord> ResourceLogs) GetLogs(
        this DistributedApplication app)
    {
        var environment = app.Services.GetRequiredService<IHostEnvironment>();
        var logCollector = app.Services.GetFakeLogCollector();
        var logs = logCollector.GetSnapshot();
        var appHostLogs = logs.Where(l => l.Category?.StartsWith($"{environment.ApplicationName}.Resources") == false)
            .ToList();
        var resourceLogs = logs.Where(l => l.Category?.StartsWith($"{environment.ApplicationName}.Resources") == true)
            .ToList();

        return (appHostLogs, resourceLogs);
    }

    /// <summary>
    /// Asserts that no errors were logged by the application or any of its resources.
    /// </summary>
    /// <remarks>
    /// Some resource types are excluded from this check because they tend to write to stderr for various non-error reasons.
    /// </remarks>
    /// <param name="app"></param>
    public static void EnsureNoErrorsLogged(this DistributedApplication app)
    {
        var (appHostLogs, resourceLogs) = app.GetLogs();

        appHostLogs.ShouldNotContain(log => log.Level >= LogLevel.Error);
        resourceLogs.ShouldNotContain(log =>
            log.Category != null && log.Category.Length > 0 && log.Level >= LogLevel.Error);
    }

    /// <summary>
    /// Creates an <see cref="HttpClient"/> configured to communicate with the specified resource.
    /// </summary>
    public static HttpClient CreateHttpClient(this DistributedApplication app, string resourceName,
        bool useHttpClientFactory)
        => app.CreateHttpClient(resourceName, null, useHttpClientFactory);

    /// <summary>
    /// Creates an <see cref="HttpClient"/> configured to communicate with the specified resource.
    /// </summary>
    public static HttpClient CreateHttpClient(this DistributedApplication app, string resourceName,
        string? endpointName, bool useHttpClientFactory)
    {
        if (useHttpClientFactory)
        {
            return app.CreateHttpClient(resourceName, endpointName);
        }

        // Don't use the HttpClientFactory to create the HttpClient so, e.g., no resilience policies are applied
        var httpClient = new HttpClient
        {
            BaseAddress = app.GetEndpoint(resourceName, endpointName)
        };

        return httpClient;
    }

    /// <summary>
    /// Creates an <see cref="HttpClient"/> configured to communicate with the specified resource with custom configuration.
    /// </summary>
    public static HttpClient CreateHttpClient(this DistributedApplication app, string resourceName,
        string? endpointName, Action<IHttpClientBuilder> configure)
    {
        var services = new ServiceCollection()
            .AddHttpClient()
            .ConfigureHttpClientDefaults(configure)
            .BuildServiceProvider();
        var httpClientFactory = services.GetRequiredService<IHttpClientFactory>();

        var httpClient = httpClientFactory.CreateClient();
        httpClient.BaseAddress = app.GetEndpoint(resourceName, endpointName);

        return httpClient;
    }
}