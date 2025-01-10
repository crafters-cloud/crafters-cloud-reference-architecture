using CraftersCloud.ReferenceArchitecture.Domain.Users;

namespace CraftersCloud.ReferenceArchitecture.Domain.Entities;

public interface IEntityWithCreatedUpdated
{
    public UserId CreatedById { get; }
    public UserId UpdatedById { get; }

    void SetCreated(DateTimeOffset createdOn, UserId createdBy);

    void SetUpdated(DateTimeOffset updatedOn, UserId updatedBy);
}