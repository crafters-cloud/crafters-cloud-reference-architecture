using System.Text.Json;
using CraftersCloud.ReferenceArchitecture.Domain.Users;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.HelloWorld;

public static class HelloWorld
{
    [PublicAPI]
    public class LineItem
    {
        public string ItemId { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }

    [PublicAPI]
    [UsedImplicitly]
    public class LineItems(List<LineItem> items) : IParsable<LineItems>
    {
        public List<LineItem> Items { get; } = items;

        public static LineItems Parse(string s, IFormatProvider? provider)
        {
            if (!TryParse(s, provider, out var request))
            {
                throw new FormatException("Cannot parse the provided input into a valid Request object.");
            }

            return request;
        }

        public static bool TryParse(string? s, IFormatProvider? provider, out LineItems result)
        {
            result = null!;

            try
            {
                var items = s != null ? JsonSerializer.Deserialize<List<LineItem>>(s)! : [];
                result = new LineItems(items);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    [PublicAPI]
    public class Response
    {
        public UserId UserId { get; set; }
        public required int ItemsCount { get; init; }
        public required int ItemsTotalQuantity { get; init; }
    };

    public static async Task<Ok<Response>> Handle([FromQuery] UserId userId, [FromQuery] LineItems lineItems,
        ILogger<HelloWorldEndpoints> logger)
    {
        logger.LogInformation("Items count: {ItemsCount}", lineItems.Items.Count);
        await Task.CompletedTask;
        return TypedResults.Ok(new Response
        {
            UserId = userId, ItemsCount = lineItems.Items.Count, ItemsTotalQuantity = lineItems.Items.Sum(x => x.Quantity)
        });
    }
}