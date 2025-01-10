using CraftersCloud.Core.Data;
using CraftersCloud.ReferenceArchitecture.Core.Cqrs;
using MediatR;

namespace CraftersCloud.ReferenceArchitecture.Domain.Products.Commands;

public class CreateProductCommandHandler(IRepository<Product, ProductId> repository)
    : IRequestHandler<CreateProductCommand, CreateCommandResult<Product>>
{
    public Task<CreateCommandResult<Product>> Handle(CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        var product = Product.Create(command);
        repository.Add(product);
        var result = CommandResult.Created(product);
        return Task.FromResult(result);
    }
}