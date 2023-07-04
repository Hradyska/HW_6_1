using System.ComponentModel.DataAnnotations;

namespace Catalog.Host.Models.Requests
{
    public class CreateTypeRequest
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(maximumLength: 100, ErrorMessage = "Maximum lenth of the field Type is 100 characters.")]
        public string Type { get; set; } = null!;
    }
}
