using CraftersCloud.Core.Data;
using CraftersCloud.Core.EntityFramework.Infrastructure.MediatR;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Data;

public class AppSaveChangesBehavior<TRequest, TResponse>(
    AppDbContext dbContext,
    IUnitOfWork unitOfWork,
    ILogger<SaveChangesBehavior<AppDbContext, TRequest, TResponse>> logger)
    : SaveChangesBehavior<AppDbContext, TRequest, TResponse>(dbContext, unitOfWork, logger)
    where TRequest : IBaseRequest;