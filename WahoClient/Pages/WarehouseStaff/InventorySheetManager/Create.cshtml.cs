using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjects.WahoModels;

namespace WahoClient.Pages.WarehouseStaff.InventorySheetManager
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
        ViewData["UserName"] = new SelectList(_context.Employees, "UserName", "UserName");
        ViewData["WahoId"] = new SelectList(_context.WahoInformations, "WahoId", "WahoId");
            return Page();
        }

        [BindProperty]
        public InventorySheet InventorySheet { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.InventorySheets == null || InventorySheet == null)
            {
                return Page();
            }

            _context.InventorySheets.Add(InventorySheet);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
