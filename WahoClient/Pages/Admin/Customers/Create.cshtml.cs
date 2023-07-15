using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjects.WahoModels;
using Waho.DataService;
using ViewModels.EmployeeViewModels;
using System.Net.WebSockets;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;
using AutoMapper.Execution;

namespace WahoClient.Pages.Admin.Customers
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient client = null;
        private string customerAPIUrl = "";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Author _author;

        public CreateModel(Author author, IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            customerAPIUrl = "https://localhost:7019/waho/Customer";
            _author = author;
            _httpContextAccessor = httpContextAccessor;
        }
        [BindProperty]
        public Customer Customer { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            //author
            if (!_author.IsAuthor(1))
            {
                return RedirectToPage("/accessDenied", new { message = "Trình quản lý của Admin" });
            }
            return Page();
        }
        public string message { get; set; }
        public string successMessage { get; set; }
        [BindProperty]
        public Employee Employee { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            // get data from session
            var employeeJson = _httpContextAccessor.HttpContext.Session.GetString("Employee");
            EmployeeVM employeeVM = JsonConvert.DeserializeObject<EmployeeVM>(employeeJson);
            var req = HttpContext.Request;
            //get data form form submit 
            string name = HttpContext.Request.Form["name"];
            string raw_dob = HttpContext.Request.Form["dob"];
            string phone = HttpContext.Request.Form["phone"];
            string email = HttpContext.Request.Form["email"];
            string raw_type = HttpContext.Request.Form["type"];
            string tax = HttpContext.Request.Form["tax"];
            string address = HttpContext.Request.Form["address"];
            string note = HttpContext.Request.Form["note"];

            Customer.CustomerName = name;
            Customer.Adress = address;
            Customer.Phone = phone;
            Customer.Email = email;
            Customer.Active = true;
            Customer.Description = note;
            Customer.TaxCode = tax;
            if (!string.IsNullOrEmpty(raw_dob))
            {
                Customer.Dob = DateTime.Parse(raw_dob);
            }
            Customer.TypeOfCustomer = Boolean.Parse(raw_type);
            Customer.WahoId = employeeVM.WahoId;

            // call api add new customer
            var json = JsonConvert.SerializeObject(Customer);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(customerAPIUrl, content);
            string messageResponse = await response.Content.ReadAsStringAsync();

            //message
            successMessage = messageResponse;
            TempData["successMessage"] = successMessage;
            return RedirectToPage("./Index");
        }
    }
}
