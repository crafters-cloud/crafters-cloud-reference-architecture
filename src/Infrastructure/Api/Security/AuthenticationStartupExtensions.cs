﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Api.Security;

public static class AuthenticationStartupExtensions
{
    public const string AzureAdSection = "App:AzureAd";

    public static void AppAddAuthentication(this IServiceCollection services, IConfiguration configuration) =>
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(configuration.GetSection(AzureAdSection));
}