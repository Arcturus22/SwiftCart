using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwiftCart.Data
{
    public class Product
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; }
        [Range(0.01,1000)] public decimal Price { get; set; }

        public string? Description { get; set; }
        public string? SpecialTag { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid category.")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public string? ImageUrl { get; set; }
    }
}
