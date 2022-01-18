using System.ComponentModel.DataAnnotations;

namespace practise.DTO
{
    public class ProductDto
    {
        [Required]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public decimal ProductPrice { get; set; }
        [Required]
        public int PersonId { get; set; }
    }
}
