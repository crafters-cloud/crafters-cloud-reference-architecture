﻿using System.Net;
using CraftersCloud.Core.Paging;
using CraftersCloud.ReferenceArchitecture.Api.Endpoints.Products;
using CraftersCloud.ReferenceArchitecture.Api.Tests.Infrastructure.Api;
using CraftersCloud.ReferenceArchitecture.Domain.Products;
using CraftersCloud.ReferenceArchitecture.Domain.Tests.Products;
using FluentAssertions;
using Flurl.Http;
using GetProducts = CraftersCloud.ReferenceArchitecture.Api.Endpoints.Products.GetProducts;
using UpdateProduct = CraftersCloud.ReferenceArchitecture.Api.Endpoints.Products.UpdateProduct;

namespace CraftersCloud.ReferenceArchitecture.Api.Tests.Endpoints;

[Category("integration")]
public class ProductEndpointsFixture : IntegrationFixtureBase
{
    private IFlurlRequest _endpoint = null!;

    private Product _product = null!;

    [SetUp]
    public void SetUp()
    {
        _endpoint = Client.Request("products");

        _product = new ProductBuilder()
            .WithName("Product 1")
            .WithDescription("Product 1 description")
            .WithStatusId(ProductStatusId.Active);

        AddAndSaveChanges(_product);
    }

    [Test]
    public async Task GetAll()
    {
        var response = await _endpoint
            .AppendQueryParam(nameof(GetProducts.Request.SortBy), nameof(Product.Name))
            .AppendQueryParam(nameof(GetProducts.Request.ProductStatusId), ProductStatusId.Active.Value)
            .GetJsonAsync<PagedQueryResponse<GetProducts.Response.Item>>();
        await Verify(response);
    }

    [Test]
    public async Task GetById()
    {
        var response = await _endpoint.AppendPathSegment(_product.Id)
            .GetJsonAsync<GetProductById.Response>();
        await Verify(response);
    }

    [Test]
    public async Task GetStatuses()
    {
        var response = await _endpoint.AppendPathSegment("statuses")
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
            await _endpoint.PostJsonAsync(request);
        response.StatusCode.Should().Be((int) HttpStatusCode.Created);
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
        var response = await _endpoint.PutJsonAsync(request);
        response.StatusCode.Should().Be((int) HttpStatusCode.NoContent);
        var product = QueryDbSkipCache<Product>().QueryById(_product.Id).Single();
        await Verify(product);
    }
}