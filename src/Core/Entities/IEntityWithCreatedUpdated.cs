namespace CraftersCloud.ReferenceArchitecture.Core.Entities;

public interface IEntityWithCreatedUpdated
{
    void SetCreated(DateTimeOffset createdOn, Guid createdBy);

    void SetUpdated(DateTimeOffset updatedOn, Guid updatedBy);
}