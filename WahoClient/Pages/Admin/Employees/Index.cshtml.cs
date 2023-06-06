using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.WahoModels;

namespace WahoClient.Pages.Admin.Employees
{
    public class IndexModel : PageModel
    {
        private readonly BusinessObjects.WahoModels.WahoS8Context _context;

        public IndexModel(BusinessObjects.WahoModels.WahoS8Context context)
        {
            _context = context;
        }

        public IList<Employee> Employee { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Employees != null)
            {
                Employee = await _context.Employees
                .Include(e => e.Waho).ToListAsync();
            }
        }
    }
}
