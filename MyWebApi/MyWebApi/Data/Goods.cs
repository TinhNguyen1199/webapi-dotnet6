using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApi.Data
{
    [Table("Goods")]
    public class Goods
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(0, double.MaxValue)]
        public double Price { get; set; }
        public byte Sale { get; set; }
        public int? CaterogyId { get; set; }
        [ForeignKey("CaterogyId")]
        public Category Category { get; set; }
    }
}
