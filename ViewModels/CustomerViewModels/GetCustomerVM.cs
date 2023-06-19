using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CustomerViewModels
{
    public class GetCustomerVM
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string? Phone { get; set; }
        public DateTime? Dob { get; set; }
        public string? Adress { get; set; }
        public bool? TypeOfCustomer { get; set; }
        public string? TaxCode { get; set; }
        public string? Email { get; set; }
        public bool? Active { get; set; }
    }
}
