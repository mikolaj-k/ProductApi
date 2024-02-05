using Microsoft.EntityFrameworkCore;

namespace ProductApi.Models.Staging
{
    [Keyless]
    public class StgInventory
    {
        public string? product_id { get; set; }
        public string? sku { get; set; }
        public string? unit { get; set; }
        public string? qty { get; set; }
        public string? manufacturer_name { get; set; }
        public string? manufacturer_ref_num { get; set; }
        public string? shipping { get; set; }
        public string? shipping_cost { get; set; }
    }
}
