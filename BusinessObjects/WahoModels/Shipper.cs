using System;
using System.Collections.Generic;

namespace BusinessObjects.WahoModels
{
    public partial class Shipper
    {
        public Shipper()
        {
            Oders = new HashSet<Oder>();
        }

        public int ShipperId { get; set; }
        public string ShipperName { get; set; }
        public string Phone { get; set; }
        public int? WahoId { get; set; }

        public virtual WahoInformation Waho { get; set; }
        public virtual ICollection<Oder> Oders { get; set; }
    }
}
