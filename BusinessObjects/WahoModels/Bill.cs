﻿using System;
using System.Collections.Generic;

namespace BusinessObjects.WahoModels
{
    public partial class Bill
    {
        public Bill()
        {
            BillDetails = new HashSet<BillDetail>();
        }

        public int BillId { get; set; }
        public DateTime Date { get; set; }
        public bool? Active { get; set; }
        public string? Descriptions { get; set; }
        public string? BillStatus { get; set; }
        public decimal? Total { get; set; }
        public int WahoId { get; set; }
        public string UserName { get; set; }
        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Employee UserNameNavigation { get; set; }
        public virtual WahoInformation Waho { get; set; }
        public virtual ICollection<BillDetail> BillDetails { get; set; }
    }
}
