using System.ComponentModel.DataAnnotations;

namespace MyWebApi.Models
{
    public class CategoryModel
    {
        [Required]
        [MaxLength(50)]
        public string CategoryName { get; set; }
    }
}
