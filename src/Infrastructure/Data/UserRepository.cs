using CraftersCloud.Core.EntityFramework.Infrastructure;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Data;

// This is an example of a Custom repository implementation.
public class UserRepository(DbContext context) : EntityFrameworkRepository<User, UserId>(context);