﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.WahoModels;

namespace WahoClient.Pages.Admin.Ships
{
    public class EditModel : PageModel
    {
        private readonly BusinessObjects.WahoModels.WahoS8Context _context;

        public EditModel(BusinessObjects.WahoModels.WahoS8Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Shipper Shipper { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Shippers == null)
            {
                return NotFound();
            }

            var shipper =  await _context.Shippers.FirstOrDefaultAsync(m => m.ShipperId == id);
            if (shipper == null)
            {
                return NotFound();
            }
            Shipper = shipper;
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

            _context.Attach(Shipper).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShipperExists(Shipper.ShipperId))
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

        private bool ShipperExists(int id)
        {
          return (_context.Shippers?.Any(e => e.ShipperId == id)).GetValueOrDefault();
        }
    }
}
