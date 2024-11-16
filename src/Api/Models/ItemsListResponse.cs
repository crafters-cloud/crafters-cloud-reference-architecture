namespace CraftersCloud.ReferenceArchitecture.Api.Models;

public class ItemsListResponse<T>
{
    public List<T> Items { get; set; } = [];
}

public static class ItemsListResponse
{
    public static T Create<T, TItem>(List<TItem> items) where T : ItemsListResponse<TItem>, new() =>
        new() { Items = items };
}