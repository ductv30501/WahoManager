using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.WahoModels;

namespace WahoClient.Pages.Cashier.Orders
{
    public class IndexModel : PageModel
    {
        private readonly BusinessObjects.WahoModels.WahoS8Context _context;

        public IndexModel(BusinessObjects.WahoModels.WahoS8Context context)
        {
            _context = context;
        }

        public IList<Oder> Oder { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Oders != null)
            {
                Oder = await _context.Oders
                .Include(o => o.Customer)
                .Include(o => o.Shipper)
                .Include(o => o.UserNameNavigation)
                .Include(o => o.Waho).ToListAsync();
            }
        }
    }
}
