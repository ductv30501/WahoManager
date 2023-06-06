using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.WahoModels;

namespace WahoClient.Pages.WarehouseStaff.Products
{
    public class EditModel : PageModel
    {
        private readonly BusinessObjects.WahoModels.WahoS8Context _context;

        public EditModel(BusinessObjects.WahoModels.WahoS8Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product =  await _context.Products.FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            Product = product;
           ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "LocationId");
           ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "SubCategoryId", "SubCategoryId");
           ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierId");
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

            _context.Attach(Product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.ProductId))
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

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
