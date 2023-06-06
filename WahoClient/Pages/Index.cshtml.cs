using BusinessObjects.WahoModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;
using ViewModels.EmployeeViewModels;
using Waho.DataService;

namespace WahoClient.Pages
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient client = null;
        private string employeeAPIUrl = "";
        public IndexModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            employeeAPIUrl = "https://localhost:7019/waho/Employee";
        }

        [BindProperty]
        public Employee Employee { get; set; } = default!;

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            HttpResponseMessage response = await client.GetAsync(employeeAPIUrl+ "/login?username="+ Employee.UserName + "&password="+ Employee.Password);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            EmployeeVM employeeVM = JsonSerializer.Deserialize<EmployeeVM>(strData,options);
            if (employeeVM != null)
            {
                HttpContext.Session.SetString("Employee", JsonSerializer.Serialize(employeeVM));
                switch (employeeVM.Role)
                {
                    case 1:
                        return RedirectToPage("./Admin/Index");
                    case 2:
                        return RedirectToPage("./Cashier/Bills/Index");
                    case 3:
                        return RedirectToPage("./WarehouseStaff/Products/Index");
                }

            }
            ModelState.AddModelError("", "Sai tên đăng nhập hoặc mật khẩu");
            return Page();
        }
    }
}