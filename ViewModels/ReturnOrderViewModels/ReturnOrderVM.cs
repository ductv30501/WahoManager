using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ReturnOrderViewModels
{
    public class ReturnOrderVM
    {
        public int ReturnOrderId { get; set; }
        public bool? State { get; set; }
        public DateTime? Date { get; set; }
        public string? Description { get; set; }
        public bool? Active { get; set; }
        public decimal PayCustomer { get; set; }
        public decimal PaidCustomer { get; set; }
        public int? BillId { get; set; }
        public string UserName { get; set; }
        public int CustomerId { get; set; }
        public int WahoId { get; set; }
    }
}
