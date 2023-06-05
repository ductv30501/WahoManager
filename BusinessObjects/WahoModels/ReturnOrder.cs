﻿using System;
using System.Collections.Generic;

namespace BusinessObjects.WahoModels
{
    public partial class ReturnOrder
    {
        public ReturnOrder()
        {
            ReturnOrderProducts = new HashSet<ReturnOrderProduct>();
        }

        public int ReturnOrderId { get; set; }
        public bool? State { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public decimal? PayCustomer { get; set; }
        public decimal? PaidCustomer { get; set; }
        public int? BillId { get; set; }
        public string UserName { get; set; }
        public int? CustomerId { get; set; }
        public int? WahoId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Employee UserNameNavigation { get; set; }
        public virtual WahoInformation Waho { get; set; }
        public virtual ICollection<ReturnOrderProduct> ReturnOrderProducts { get; set; }
    }
}
