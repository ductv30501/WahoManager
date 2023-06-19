using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ViewModels.CustomerViewModels
{
    public class PostCustomerVM
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string? Phone { get; set; }
        public DateTime? Dob { get; set; }
        public string? Adress { get; set; }
        public bool? TypeOfCustomer { get; set; }
        public string? TaxCode { get; set; }
        public string? Email { get; set; }
        public string? Description { get; set; }
        public bool? Active { get; set; }
        public int WahoId { get; set; }
    }
}
