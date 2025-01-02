namespace CraftersCloud.ReferenceArchitecture.Api.Models;

public class ItemsResponse<TItem>
{
    public List<TItem> Items { get; set; } = [];
}