using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.InventorySheetViewModels
{
    public class InventorySheetVM
    {
        public int InventorySheetId { get; set; }
        public string? Description { get; set; }
        public DateTime? Date { get; set; }
        public bool? Active { get; set; }
        public int WahoId { get; set; }
        public string UserName { get; set; }
    }
}
