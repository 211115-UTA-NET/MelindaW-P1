using PlainOldStoreApp.DataStorage;
using System.ComponentModel.DataAnnotations;

namespace PlainOldStoreApi.Api.Dtos
{
    public class AddOrder
    {
        [Required]
        public Guid CustomerId { get; set; }
        [Required]
        public int StoreLocation { get; set; }
        [Required]
        public int? ProductId { get; set; }
        [Required]
        public decimal ProductPrice { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public int? ProductQuantiy { get; set; }
    }
}
