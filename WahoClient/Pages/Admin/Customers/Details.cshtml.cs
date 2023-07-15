using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.WahoModels;
using Newtonsoft.Json;
using System.Text;

namespace WahoClient.Pages.Admin.Customers
{
    public class DetailsModel : PageModel
    {
        private readonly BusinessObjects.WahoModels.WahoS8Context _context;

        public DetailsModel(BusinessObjects.WahoModels.WahoS8Context context)
        {
            _context = context;
        }

      public Customer Customer { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            return Page();
        }
    }
}
