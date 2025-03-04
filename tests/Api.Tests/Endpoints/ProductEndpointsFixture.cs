﻿using CraftersCloud.Core.Entities;
using CraftersCloud.Core.Paging;
using CraftersCloud.ReferenceArchitecture.Api.Endpoints.Products;
using CraftersCloud.ReferenceArchitecture.Domain.Products;
using CraftersCloud.ReferenceArchitecture.Domain.Tests.Products;

namespace CraftersCloud.ReferenceArchitecture.Api.Tests.Endpoints;

[Category("integration")]
public class ProductEndpointsFixture : EndpointsFixtureBase
{
    private IFlurlRequest Endpoint =>  Client.Request("products");

    private Product _product = null!;

    [SetUp]
    public void SetUp()
    {
        _product = new ProductBuilder()
            .WithName("Product 1")
            .WithDescription("Product 1 description")
            .WithStatusId(ProductStatusId.Active);

        AddAndSaveChanges(_product);
    }

    [Test]
    public async Task GetAll()
    {
        var response = await Endpoint
            .AppendQueryParam(nameof(GetProducts.Request.SortBy), nameof(Product.Name))
            .AppendQueryParam(nameof(GetProducts.Request.ProductStatusId), ProductStatusId.Active.Value)
            .GetJsonAsync<PagedQueryResponse<GetProducts.Response.Item>>();
        await Verify(response);
    }

    [Test]
    public async Task GetById()
    {
        var response = await Endpoint.AppendPathSegment(_product.Id)
            .GetJsonAsync<GetProductById.Response>();
        await Verify(response);
    }

    [Test]
    public async Task GetStatuses()
    {
        var response = await Endpoint.AppendPathSegment("statuses")
            .GetJsonAsync<GetProductStatuses.Response>();
        await Verify(response);
    }

    [Test]
    public async Task Create()
    {
        var request = new CreateProduct.Request(
            "a new product",
            "a new product description",
            ProductStatusId.Active
        );
        var response =
            await Endpoint.PostJsonAsync(request);
        response.StatusCode.ShouldBe((int) HttpStatusCode.Created);
    }

    [Test]
    public async Task Update()
    {
        var request = new UpdateProduct.Request(
            _product.Id,
            "new name",
            "new description",
            ProductStatusId.Inactive
        );
        var response = await Endpoint.PutJsonAsync(request);
        response.StatusCode.ShouldBe((int) HttpStatusCode.NoContent);
        var product = QueryDbSkipCache<Product>().QueryById(_product.Id).Single();
        await Verify(product);
    }
}