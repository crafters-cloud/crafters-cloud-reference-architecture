using CraftersCloud.Core.EntityFramework.Infrastructure;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Data;

public class UserRepository(DbContext context) : EntityFrameworkRepository<User, Guid>(context);