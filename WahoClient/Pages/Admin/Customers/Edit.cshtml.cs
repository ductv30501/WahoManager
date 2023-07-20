using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.WahoModels;
using System.Net.Http.Headers;
using Waho.DataService;
using Newtonsoft.Json;
using ViewModels.EmployeeViewModels;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace WahoClient.Pages.Admin.Customers
{
    [Authorize(Roles = "1")]

    public class EditModel : PageModel
    {
        private readonly HttpClient client = null;
        private string customerAPIUrl = "";
        private readonly Author _author;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EditModel(Author author, IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            customerAPIUrl = "https://localhost:7019/waho/Customer";
            _author = author;
            _httpContextAccessor = httpContextAccessor;
        }
        public string message { get; set; }
        public string successMessage { get; set; }
        [BindProperty]
        public Customer Customer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            //author
            if (User.Identity?.IsAuthenticated == false)
            {
                return RedirectToPage("/accessDenied", new { message = "do bạn chưa đăng nhập" });
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Request.Cookies["AccessToken"]);

            // get data from session
            var employeeJson = _httpContextAccessor.HttpContext.Session.GetString("Employee");
            EmployeeVM eSession = JsonConvert.DeserializeObject<EmployeeVM>(employeeJson);

            string id = HttpContext.Request.Form["id"];
            string name = HttpContext.Request.Form["name"];
            string raw_dob = HttpContext.Request.Form["dob"];
            string phone = HttpContext.Request.Form["phone"];
            string email = HttpContext.Request.Form["email"];
            string raw_type = HttpContext.Request.Form["type"];
            string tax = HttpContext.Request.Form["tax"];
            string address = HttpContext.Request.Form["address"];
            string note = HttpContext.Request.Form["note"];
            string active = HttpContext.Request.Form["active"];

            Customer.CustomerId = int.Parse(id);
            Customer.CustomerName = name;
            Customer.Adress = address;
            Customer.Phone = phone;
            Customer.Email = email;
            Customer.Active = true;
            Customer.Description = note;
            Customer.TaxCode = tax;
            Customer.Dob = DateTime.Parse(raw_dob);
            Customer.TypeOfCustomer = Boolean.Parse(raw_type);
            Customer.Active = bool.Parse(active);
            Customer.WahoId = eSession.WahoId;

            //update to data
            var json = JsonConvert.SerializeObject(Customer);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(customerAPIUrl, content);
            if ((int)response.StatusCode == 401) return RedirectToPage("/accessDenied");

            string messageResponse = await response.Content.ReadAsStringAsync();

            TempData["successMessage"] = messageResponse;
            return RedirectToPage("./Index");
        }
    }
}
