using System.ComponentModel.DataAnnotations;

namespace Basket.Host.Models.Requests
{
    public class AddRequest<T>
    {
        [Required]
        public T Data { get; set; } = default(T)!;
    }
}
