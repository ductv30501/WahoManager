using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.WahoModels;

namespace WahoClient.Pages.WarehouseStaff.InventorySheetManager
{
    public class EditModel : PageModel
    {
        private readonly BusinessObjects.WahoModels.WahoS8Context _context;

        public EditModel(BusinessObjects.WahoModels.WahoS8Context context)
        {
            _context = context;
        }

        [BindProperty]
        public InventorySheet InventorySheet { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.InventorySheets == null)
            {
                return NotFound();
            }

            var inventorysheet =  await _context.InventorySheets.FirstOrDefaultAsync(m => m.InventorySheetId == id);
            if (inventorysheet == null)
            {
                return NotFound();
            }
            InventorySheet = inventorysheet;
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

            _context.Attach(InventorySheet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventorySheetExists(InventorySheet.InventorySheetId))
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

        private bool InventorySheetExists(int id)
        {
          return (_context.InventorySheets?.Any(e => e.InventorySheetId == id)).GetValueOrDefault();
        }
    }
}
