﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.WahoModels;

namespace WahoClient.Pages.Admin.Suppliers
{
    public class IndexModel : PageModel
    {
        private readonly BusinessObjects.WahoModels.WahoS8Context _context;

        public IndexModel(BusinessObjects.WahoModels.WahoS8Context context)
        {
            _context = context;
        }

        public IList<Supplier> Supplier { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Suppliers != null)
            {
                Supplier = await _context.Suppliers
                .Include(s => s.Waho).ToListAsync();
            }
        }
    }
}
