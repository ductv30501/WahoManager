using System;
using System.Collections.Generic;

namespace BusinessObjects.WahoModels
{
    public partial class WahoInformation
    {
        public WahoInformation()
        {
            Bills = new HashSet<Bill>();
            Customers = new HashSet<Customer>();
            Employees = new HashSet<Employee>();
            InventorySheets = new HashSet<InventorySheet>();
            Oders = new HashSet<Oder>();
            Products = new HashSet<Product>();
            ReturnOrders = new HashSet<ReturnOrder>();
            Shippers = new HashSet<Shipper>();
            Suppliers = new HashSet<Supplier>();
        }

        public int WahoId { get; set; }
        public string WahoName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool? Active { get; set; }
        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Bill> Bills { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<InventorySheet> InventorySheets { get; set; }
        public virtual ICollection<Oder> Oders { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<ReturnOrder> ReturnOrders { get; set; }
        public virtual ICollection<Shipper> Shippers { get; set; }
        public virtual ICollection<Supplier> Suppliers { get; set; }
    }
}
