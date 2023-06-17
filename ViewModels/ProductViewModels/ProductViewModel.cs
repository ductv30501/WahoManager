using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ProductViewModels
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int? ImportPrice { get; set; }
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }
        public bool? HaveDate { get; set; }
        public DateTime? DateOfManufacture { get; set; }
        public DateTime? Expiry { get; set; }
        public string? Trademark { get; set; }
        public int? Weight { get; set; }
        public string? Unit { get; set; }
        public int? InventoryLevelMin { get; set; }
        public int? InventoryLevelMax { get; set; }
        public string? Description { get; set; }
        public bool Active { get; set; }
        public int LocationId { get; set; }
        public int SubCategoryId { get; set; }
        public int WahoId { get; set; }
        public int SupplierId { get; set; }
    }
}
