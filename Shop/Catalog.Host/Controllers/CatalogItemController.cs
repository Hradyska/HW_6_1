using System.Net;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Response;
using Catalog.Host.Services.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers;

[ApiController]
[Route(ComponentDefaults.DefaultRoute)]
public class CatalogItemController : ControllerBase
{
    private readonly ILogger<CatalogItemController> _logger;
    private readonly ICatalogItemService _catalogItemService;

    public CatalogItemController(
        ILogger<CatalogItemController> logger,
        ICatalogItemService catalogItemService)
    {
        _logger = logger;
        _catalogItemService = catalogItemService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(AddItemResponse<int?>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Add(CreateProductRequest request)
    {
        CatalogItem item = new CatalogItem()
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            AvailableStock = request.AvailableStock,
            CatalogBrandId = request.CatalogBrandId,
            CatalogTypeId = request.CatalogTypeId,
            PictureFileName = request.PictureFileName
        };

        var result = await _catalogItemService.Add(item);
        return Ok(new AddItemResponse<int?>() { Id = result });
    }

    [HttpPost]
    [Route("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> Remove(int id)
    {
        var result = await _catalogItemService.Remove(id);
        return Ok(new AddItemResponse<int?>() { Id = result });
    }

    [HttpPost]
    [Route("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(int id, CreateProductRequest request)
    {
        CatalogItem item = new CatalogItem()
        {
            Id = id,
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            AvailableStock = request.AvailableStock,
            CatalogBrandId = request.CatalogBrandId,
            CatalogTypeId = request.CatalogTypeId,
            PictureFileName = request.PictureFileName
        };
        var result = await _catalogItemService.Update(item);
        return Ok(new AddItemResponse<int?>() { Id = result });
    }
}
