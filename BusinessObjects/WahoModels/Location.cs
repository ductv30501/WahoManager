using System;
using System.Collections.Generic;

namespace BusinessObjects.WahoModels
{
    public partial class Location
    {
        public Location()
        {
            Products = new HashSet<Product>();
        }

        public int LocationId { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
