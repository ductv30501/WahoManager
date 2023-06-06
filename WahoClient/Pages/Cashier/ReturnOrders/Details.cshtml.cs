using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.WahoModels;

namespace WahoClient.Pages.Cashier.ReturnOrders
{
    public class DetailsModel : PageModel
    {
        private readonly BusinessObjects.WahoModels.WahoS8Context _context;

        public DetailsModel(BusinessObjects.WahoModels.WahoS8Context context)
        {
            _context = context;
        }

      public ReturnOrder ReturnOrder { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.ReturnOrders == null)
            {
                return NotFound();
            }

            var returnorder = await _context.ReturnOrders.FirstOrDefaultAsync(m => m.ReturnOrderId == id);
            if (returnorder == null)
            {
                return NotFound();
            }
            else 
            {
                ReturnOrder = returnorder;
            }
            return Page();
        }
    }
}
