using CraftersCloud.Core.Data;
using CraftersCloud.ReferenceArchitecture.Core.Cqrs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.ReferenceArchitecture.Domain.Products.Commands;

public class UpdateProductCommandHandler(IRepository<Product, ProductId> repository)
    : IRequestHandler<UpdateProductCommand, UpdateCommandResult<Product>>
{
    public async Task<UpdateCommandResult<Product>> Handle(UpdateProductCommand command,
        CancellationToken cancellationToken)
    {
        var product = await repository.QueryAll().QueryById(command.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (product == null)
        {
            return CommandResult.UpdateNotFound<Product>();
        }

        product.Update(command);
        return CommandResult.UpdateSuccess<Product>();
    }
}