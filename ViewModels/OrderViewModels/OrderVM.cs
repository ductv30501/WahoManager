using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.OrderViewModels
{
    public class OrderVM
    {
        public int OderId { get; set; }
        public string? OderState { get; set; }
        public string? Region { get; set; }
        public string? Cod { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? EstimatedDate { get; set; }
        public decimal Total { get; set; }
        public decimal Deposit { get; set; }
        public bool? Active { get; set; }
        public string UserName { get; set; }
        public int CustomerId { get; set; }
        public int ShipperId { get; set; }
        public int WahoId { get; set; }
    }
}
