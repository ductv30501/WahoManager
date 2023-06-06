using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.WahoModels;

namespace WahoClient.Pages.Cashier.Orders
{
    public class EditModel : PageModel
    {
        private readonly BusinessObjects.WahoModels.WahoS8Context _context;

        public EditModel(BusinessObjects.WahoModels.WahoS8Context context)
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

            var oder =  await _context.Oders.FirstOrDefaultAsync(m => m.OderId == id);
            if (oder == null)
            {
                return NotFound();
            }
            Oder = oder;
           ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerName");
           ViewData["ShipperId"] = new SelectList(_context.Shippers, "ShipperId", "ShipperId");
           ViewData["UserName"] = new SelectList(_context.Employees, "UserName", "UserName");
           ViewData["WahoId"] = new SelectList(_context.WahoInformations, "WahoId", "WahoId");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Oder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OderExists(Oder.OderId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool OderExists(int id)
        {
          return (_context.Oders?.Any(e => e.OderId == id)).GetValueOrDefault();
        }
    }
}
