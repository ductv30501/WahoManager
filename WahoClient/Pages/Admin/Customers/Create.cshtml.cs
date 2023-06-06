using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjects.WahoModels;

namespace WahoClient.Pages.Admin.Customers
{
    public class CreateModel : PageModel
    {
        private readonly BusinessObjects.WahoModels.WahoS8Context _context;

        public CreateModel(BusinessObjects.WahoModels.WahoS8Context context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["WahoId"] = new SelectList(_context.WahoInformations, "WahoId", "WahoId");
            return Page();
        }

        [BindProperty]
        public Customer Customer { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Customers == null || Customer == null)
            {
                return Page();
            }

            _context.Customers.Add(Customer);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
