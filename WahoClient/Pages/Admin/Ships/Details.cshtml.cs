using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.WahoModels;

namespace WahoClient.Pages.Admin.Ships
{
    public class DetailsModel : PageModel
    {
        private readonly BusinessObjects.WahoModels.WahoS8Context _context;

        public DetailsModel(BusinessObjects.WahoModels.WahoS8Context context)
        {
            _context = context;
        }

      public Shipper Shipper { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Shippers == null)
            {
                return NotFound();
            }

            var shipper = await _context.Shippers.FirstOrDefaultAsync(m => m.ShipperId == id);
            if (shipper == null)
            {
                return NotFound();
            }
            else 
            {
                Shipper = shipper;
            }
            return Page();
        }
    }
}
