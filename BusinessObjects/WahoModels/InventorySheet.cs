using System;
using System.Collections.Generic;

namespace BusinessObjects.WahoModels
{
    public partial class InventorySheet
    {
        public InventorySheet()
        {
            InventorySheetDetails = new HashSet<InventorySheetDetail>();
        }

        public int InventorySheetId { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public bool? Active { get; set; }
        public int? WahoId { get; set; }
        public string UserName { get; set; }

        public virtual Employee UserNameNavigation { get; set; }
        public virtual WahoInformation Waho { get; set; }
        public virtual ICollection<InventorySheetDetail> InventorySheetDetails { get; set; }
    }
}
