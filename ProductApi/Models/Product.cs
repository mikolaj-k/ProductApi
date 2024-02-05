using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductApi.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [MaxLength(20)]
        public string SKU { get; set; } = string.Empty;
        [MaxLength(20)]
        public string EAN { get; set; } = string.Empty;
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(100)]
        public string ManufacturerName { get; set; } = string.Empty;
        [MaxLength(300)]
        public string Category { get; set; } = string.Empty;
        [MaxLength(300)]
        public string? ImgUrl { get; set; }
        [MaxLength(20)]
        public string LogisticUnit { get; set; } = string.Empty;
        public Inventory? Inventory { get; set; }
        public Price? Price { get; set; }
    }
}
