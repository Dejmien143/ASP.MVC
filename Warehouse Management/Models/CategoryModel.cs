using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse_Management.Models
{
    [Table("CategoryModel")]
    public class CategoryModel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } 
       
    }
}
