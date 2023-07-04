using System.Net;
using Catalog.Host.Configurations;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Response;
using Catalog.Host.Services.Interfaces;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Catalog.Host.Models.Enums;

namespace Catalog.Host.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
[Route(ComponentDefaults.DefaultRoute)]
public class CatalogBffController : ControllerBase
{
    private readonly ILogger<CatalogBffController> _logger;
    private readonly ICatalogService _catalogService;
    private readonly IOptions<CatalogConfig> _config;

    public CatalogBffController(
        ILogger<CatalogBffController> logger,
        ICatalogService catalogService,
        IOptions<CatalogConfig> config)
    {
        _logger = logger;
        _catalogService = catalogService;
        _config = config;
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PaginatedItemsResponse<CatalogItemDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Items(PaginatedItemsRequest<CatalogTypeFilter> request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _catalogService.GetCatalogItemsAsync(request.PageSize, request.PageIndex, request.Filters);
        return Ok(result);
    }

    [HttpPost]
    [Route("{id}")]
    [ProducesResponseType(typeof(GetItemsResponse<CatalogItemDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetItemsById(GetItemsRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _catalogService.GetCatalogItemsByIdAsync(request.Id);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(GetItemsResponse<CatalogItemDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetItemsByBrand(GetItemsRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _catalogService.GetCatalogItemsByBrandAsync(request.Id);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(GetItemsResponse<CatalogItemDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetItemsByType(GetItemsRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _catalogService.GetCatalogItemsByTypeAsync(request.Id);
        return Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BrandsResponse<CatalogBrandDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Brands()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _catalogService.GetCatalogBrandsAsync();
        return Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(TypesResponse<CatalogTypeDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Types()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _catalogService.GetCatalogTypesAsync();
        return Ok(result);
    }
}