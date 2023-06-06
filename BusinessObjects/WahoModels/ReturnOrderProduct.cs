﻿using System;
using System.Collections.Generic;

namespace BusinessObjects.WahoModels
{
    public partial class ReturnOrderProduct
    {
        public int ProductId { get; set; }
        public int ReturnOrderId { get; set; }
        public int Quantity { get; set; }
        public double? Discount { get; set; }

        public virtual Product Product { get; set; }
        public virtual ReturnOrder ReturnOrder { get; set; }
    }
}
