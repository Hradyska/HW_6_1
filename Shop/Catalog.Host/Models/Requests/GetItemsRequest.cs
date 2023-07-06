using System.ComponentModel.DataAnnotations;

namespace Catalog.Host.Models.Requests
{
    public class GetItemsRequest
    {
        [Required(ErrorMessage = "The field Id is required.")]
        [Range(1, 100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Id { get; set; }
    }
}
