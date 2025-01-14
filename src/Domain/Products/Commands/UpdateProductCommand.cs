﻿using CraftersCloud.Core;
using CraftersCloud.Core.Cqrs;
using CraftersCloud.Core.Data;
using CraftersCloud.Core.Entities;
using CraftersCloud.ReferenceArchitecture.Core.Cqrs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CraftersCloud.ReferenceArchitecture.Domain.Products.Commands;

[PublicAPI]
public class UpdateProductCommand : ICommand<UpdateCommandResult<Product>>
{
    public ProductId Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ProductStatusId ProductStatusId { get; set; } = ProductStatusId.Active;

    [UsedImplicitly]
    public class Validator : AbstractValidator<UpdateProductCommand>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public Validator(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            RuleFor(x => x.Name).ValidateProductName(UniqueProductName);
            RuleFor(x => x.Description).ValidateProductDescription();
            RuleFor(x => x.ProductStatusId).ValidateProductStatusId();
        }

        private async Task<bool> UniqueProductName(UpdateProductCommand command, string name,
            CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var repository = scope.Resolve<IRepository<Product>>();
            return !await repository.QueryAll()
                .QueryExceptWithId(command.Id)
                .QueryByName(name)
                .AnyAsync(cancellationToken);
        }
    }
}