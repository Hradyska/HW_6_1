﻿using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Dtos;
using MVC.Models.Enums;
using MVC.Services.Interfaces;
using MVC.ViewModels;
using System.Collections.Generic;

namespace MVC.Services;

public class CatalogService : ICatalogService
{
    private readonly IOptions<AppSettings> _settings;
    private readonly IHttpClientService _httpClient;
    private readonly ILogger<CatalogService> _logger;

    public CatalogService(IHttpClientService httpClient, ILogger<CatalogService> logger, IOptions<AppSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings;
        _logger = logger;
    }

    public async Task<Catalog> GetCatalogItems(int page, int take, int? brand, int? type)
    {
        var filters = new Dictionary<CatalogTypeFilter, int>();

        if (brand.HasValue)
        {
            filters.Add(CatalogTypeFilter.Brand, brand.Value);
        }
        
        if (type.HasValue)
        {
            filters.Add(CatalogTypeFilter.Type, type.Value);
        }
        
        var result = await _httpClient.SendAsync<Catalog, PaginatedItemsRequest<CatalogTypeFilter>>($"{_settings.Value.CatalogUrl}/items",
           HttpMethod.Post, 
           new PaginatedItemsRequest<CatalogTypeFilter>()
            {
                PageIndex = page,
                PageSize = take,
                Filters = filters
            });

        return result;
    }

    public async Task<IEnumerable<SelectListItem>> GetBrands()
    {
        var result = await _httpClient.SendAsync<GetCatalogBrand, object > ($"{_settings.Value.CatalogUrl}/brands", HttpMethod.Post, null);
        List<SelectListItem> list = new List<SelectListItem>();
        foreach (var item in result.Data)
        {
            list.Add(new SelectListItem(text: item.Brand, value: item.Id.ToString() ));
        }
        return list;
    }

    public async Task<IEnumerable<SelectListItem>> GetTypes()
    {
        var result = await _httpClient.SendAsync<GetCatalogType, object?>($"{_settings.Value.CatalogUrl}/types", HttpMethod.Post, null);
        List<SelectListItem> list = new List<SelectListItem>();
        foreach (var item in result.Data)
        {
            list.Add(new SelectListItem(text: item.Type, value: item.Id.ToString()));
        }

        return list;
    }
}
