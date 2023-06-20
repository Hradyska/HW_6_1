using System.Net;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Response;
using Catalog.Host.Services;
using Catalog.Host.Services.Interfaces;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.AllowClientPolicy)]
[Scope("catalog.catalogbrand")]
[Route(ComponentDefaults.DefaultRoute)]
public class CatalogBrandController : ControllerBase
{
    private readonly ILogger<CatalogBrandController> _logger;
    private readonly ICatalogBrandService _catalogBrandService;

    public CatalogBrandController(ILogger<CatalogBrandController> logger, ICatalogBrandService catalogBrandService)
    {
        _logger = logger;
        _catalogBrandService = catalogBrandService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(AddBrandResponse<int?>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Add(CreateBrandRequest request)
    {
        var result = await _catalogBrandService.Add(request.Brand);
        return Ok(new AddBrandResponse<int?>() { Id = result });
    }

    [HttpPost]
    [Route("{id}")]
    [ProducesResponseType(typeof(AddBrandResponse<int?>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Remove(int id)
    {
        var result = await _catalogBrandService.Remove(id);
        return Ok(new AddBrandResponse<int?>() { Id = result });
    }

    [HttpPost]
    [Route("{id}")]
    [ProducesResponseType(typeof(AddBrandResponse<int?>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(int id, CreateBrandRequest request)
    {
        var result = await _catalogBrandService.Update(id, request.Brand);
        return Ok(new AddBrandResponse<int?>() { Id = result });
    }
}