using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ProductApi.Models
{
    public class Price
    {
        [Key]
        [MaxLength(20)]
        public string Id { get; set; } = string.Empty;
        [MaxLength(20)]
        public string ProductSKU { get; set; } = string.Empty;
        [Precision(18, 2)]
        public decimal NetPrice { get; set; }
        public Product Product { get; set; } = new Product();
    }
}
