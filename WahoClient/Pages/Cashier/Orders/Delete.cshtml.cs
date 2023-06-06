using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.WahoModels;

namespace WahoClient.Pages.Cashier.Orders
{
    public class DeleteModel : PageModel
    {
        private readonly BusinessObjects.WahoModels.WahoS8Context _context;

        public DeleteModel(BusinessObjects.WahoModels.WahoS8Context context)
        {
            _context = context;
        }

        [BindProperty]
      public Oder Oder { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Oders == null)
            {
                return NotFound();
            }

            var oder = await _context.Oders.FirstOrDefaultAsync(m => m.OderId == id);

            if (oder == null)
            {
                return NotFound();
            }
            else 
            {
                Oder = oder;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Oders == null)
            {
                return NotFound();
            }
            var oder = await _context.Oders.FindAsync(id);

            if (oder != null)
            {
                Oder = oder;
                _context.Oders.Remove(Oder);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
