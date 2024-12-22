using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [Required, MaxLength(100)] 
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        public string PictureUrl { get; set; }

        [Required, MaxLength(100)]
        public string Type { get; set; }

        [Required, MaxLength(100)]
        public string Brand { get; set; }
        [Required]
        public int QuantityInStock { get; set; }
    }
}