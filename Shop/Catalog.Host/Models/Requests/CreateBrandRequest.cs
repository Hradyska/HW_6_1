using System.ComponentModel.DataAnnotations;

namespace Catalog.Host.Models.Requests
{
    public class CreateBrandRequest
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(maximumLength: 100, ErrorMessage = "Maximum lenth of the field Brand is 100 characters.")]
        public string Brand { get; set; } = null!;
    }
}
