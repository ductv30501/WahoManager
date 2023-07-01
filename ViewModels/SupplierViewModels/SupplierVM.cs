using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.SupplierViewModels
{
    public class SupplierVM
    {
        public int SupplierId { get; set; }
        public string? CompanyName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? TaxCode { get; set; }
        public string? Branch { get; set; }
        public string? Description { get; set; }
        public bool? Active { get; set; }
        public int WahoId { get; set; }
    }
}
