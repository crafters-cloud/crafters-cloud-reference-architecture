using CraftersCloud.Core.Entities;
using CraftersCloud.ReferenceArchitecture.Domain.Users;

namespace CraftersCloud.ReferenceArchitecture.Domain.Entities;

public abstract class EntityWithCreatedUpdated<TId> : EntityWithTypedId<TId>, IEntityWithCreatedUpdated
{
    public UserId CreatedById { get; private set; }
    public UserId UpdatedById { get; private set; }
    public DateTimeOffset CreatedOn { get; private set; }
    public DateTimeOffset UpdatedOn { get; private set; }

    public void SetCreated(DateTimeOffset createdOn, UserId createdBy)
    {
        CreatedOn = createdOn;
        CreatedById = createdBy;
    }

    public void SetUpdated(DateTimeOffset updatedOn, UserId updatedBy)
    {
        UpdatedOn = updatedOn;
        UpdatedById = updatedBy;
    }
}