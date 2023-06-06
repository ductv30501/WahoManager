using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.WahoModels;

namespace WahoClient.Pages.Admin.Customers
{
    public class IndexModel : PageModel
    {
        private readonly BusinessObjects.WahoModels.WahoS8Context _context;

        public IndexModel(BusinessObjects.WahoModels.WahoS8Context context)
        {
            _context = context;
        }

        public IList<Customer> Customer { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Customers != null)
            {
                Customer = await _context.Customers
                .Include(c => c.Waho).ToListAsync();
            }
        }
    }
}
