using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ProductApi.Models
{
    public class Inventory
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        [Precision(18, 2)]
        public decimal ShippingCost { get; set; }
        public Product Product { get; set; } = new Product();
    }
}
