using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.WahoModels;

namespace WahoClient.Pages.WarehouseStaff.InventorySheetManager
{
    public class DetailsModel : PageModel
    {
        private readonly BusinessObjects.WahoModels.WahoS8Context _context;

        public DetailsModel(BusinessObjects.WahoModels.WahoS8Context context)
        {
            _context = context;
        }

      public InventorySheet InventorySheet { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.InventorySheets == null)
            {
                return NotFound();
            }

            var inventorysheet = await _context.InventorySheets.FirstOrDefaultAsync(m => m.InventorySheetId == id);
            if (inventorysheet == null)
            {
                return NotFound();
            }
            else 
            {
                InventorySheet = inventorysheet;
            }
            return Page();
        }
    }
}
