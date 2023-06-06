using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjects.WahoModels;

namespace WahoClient.Pages.Admin.Suppliers
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
        public Supplier Supplier { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Suppliers == null || Supplier == null)
            {
                return Page();
            }

            _context.Suppliers.Add(Supplier);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
