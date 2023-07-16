using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.BillViewModel
{
    public class PostBill
    {
        public int BillId { get; set; }
        public DateTime Date { get; set; }
        public bool? Active { get; set; }
        public string? Descriptions { get; set; }
        public string? BillStatus { get; set; }
        public decimal? Total { get; set; }
        public int WahoId { get; set; }
        public string UserName { get; set; }
        public int CustomerId { get; set; }
    }
}
