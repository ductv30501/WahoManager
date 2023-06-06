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
    public class IndexModel : PageModel
    {
        private readonly BusinessObjects.WahoModels.WahoS8Context _context;

        public IndexModel(BusinessObjects.WahoModels.WahoS8Context context)
        {
            _context = context;
        }

        public IList<InventorySheet> InventorySheet { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.InventorySheets != null)
            {
                InventorySheet = await _context.InventorySheets
                .Include(i => i.UserNameNavigation)
                .Include(i => i.Waho).ToListAsync();
            }
        }
    }
}
