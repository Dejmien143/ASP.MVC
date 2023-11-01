using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Warehouse_Management.Models
{
    [Table("ProductModel")]
    public class ProductModel
    {
        [Key]
        public int Id { get; set; }
        
        [StringLength(8, ErrorMessage = "Name must be 8 characters or less.")]
        [Required]
        public string Code { get; set; }
        [Required]
        [StringLength(16, ErrorMessage = "Name must be 16 characters or less.")]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
                      
        [Required]
        [ForeignKey("CategoryModel")]
        public CategoryModel Category { get; set; }
        [Required]
        public float Quantity { get; set; }

        [Required]
        [RegularExpression("kg|pcs|litre", ErrorMessage = "Unit of measure must be one of: kg, pcs, litre")]
        public string Measure { get; set; }
        [Required]
        public float Price { get; set; }
                       
    }
}
