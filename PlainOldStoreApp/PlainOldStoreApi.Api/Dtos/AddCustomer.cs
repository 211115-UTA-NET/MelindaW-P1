using System.ComponentModel.DataAnnotations;

namespace PlainOldStoreApi.Api.Dots
{
    public class AddCustomer
    {
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? Address1 { get; set; }
        [Required]
        public string? City { get; set; }
        [Required]
        public string? State { get; set; }
        [Required]
        public string? ZipCode { get; set; }
        [Required]
        public string? Email { get; set; }
    }
}
