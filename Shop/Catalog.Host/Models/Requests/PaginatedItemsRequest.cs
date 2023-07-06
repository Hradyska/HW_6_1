using System.ComponentModel.DataAnnotations;

namespace Catalog.Host.Models.Requests;

public class PaginatedItemsRequest<T>
    where T : notnull
{
    [Required(ErrorMessage = "The field PageIndex is required.")]
    public int PageIndex { get; set; }

    [Required(ErrorMessage = "The field PageSize is required.")]
    public int PageSize { get; set; }

    public Dictionary<T, int>? Filters { get; set; }
}