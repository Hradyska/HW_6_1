using System.ComponentModel.DataAnnotations;
using Catalog.Host.Data.Entities;

namespace Catalog.Host.Models.Requests;

public class CreateProductRequest
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "The field Name is required.")]
    [StringLength(maximumLength: 50, ErrorMessage = "Maximum lenth of the field Name is 50 characters.")]
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    [Required(AllowEmptyStrings = false, ErrorMessage = "The field Price is required.")]
    public decimal Price { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "The field PictureFileName is required.")]
    public string PictureFileName { get; set; } = null!;

    public int CatalogTypeId { get; set; }

    public int CatalogBrandId { get; set; }

    public int AvailableStock { get; set; }
}