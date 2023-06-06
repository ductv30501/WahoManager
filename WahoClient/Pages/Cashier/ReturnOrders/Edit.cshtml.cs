using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.WahoModels;

namespace WahoClient.Pages.Cashier.ReturnOrders
{
    public class EditModel : PageModel
    {
        private readonly BusinessObjects.WahoModels.WahoS8Context _context;

        public EditModel(BusinessObjects.WahoModels.WahoS8Context context)
        {
            _context = context;
        }

        [BindProperty]
        public ReturnOrder ReturnOrder { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.ReturnOrders == null)
            {
                return NotFound();
            }

            var returnorder =  await _context.ReturnOrders.FirstOrDefaultAsync(m => m.ReturnOrderId == id);
            if (returnorder == null)
            {
                return NotFound();
            }
            ReturnOrder = returnorder;
           ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerName");
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

            _context.Attach(ReturnOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReturnOrderExists(ReturnOrder.ReturnOrderId))
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

        private bool ReturnOrderExists(int id)
        {
          return (_context.ReturnOrders?.Any(e => e.ReturnOrderId == id)).GetValueOrDefault();
        }
    }
}
