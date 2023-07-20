using BusinessObjects.WahoModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using ViewModels.EmployeeViewModels;
using Waho.DataService;

namespace WahoClient.Pages
{
    [AllowAnonymous]

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

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Request.Cookies["AccessToken"]);

            HttpResponseMessage response = await client.GetAsync(employeeAPIUrl + "/login?username=" + Employee.UserName + "&password=" + Employee.Password);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            EmployeeVM employeeVM = JsonSerializer.Deserialize<EmployeeVM>(strData, options);
            if (employeeVM != null)
            {
                HttpContext.Session.SetString("Employee", JsonSerializer.Serialize(employeeVM));
                
                HttpContext.Response.Cookies.Append("AccessToken", employeeVM.Token, new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(1).AddMinutes(-1)
                });
                var claims = new List<Claim>
                {
                        new Claim(ClaimTypes.NameIdentifier, employeeVM.UserName),
                        new Claim(ClaimTypes.Name, employeeVM.EmployeeName),
                        new Claim(ClaimTypes.Email, employeeVM.Email),
                        new Claim(ClaimTypes.Role, employeeVM.Role.ToString())
                };
                var claimsIdentity = new ClaimsIdentity(claims, "login");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync("CookieAuthentication", claimsPrincipal, new AuthenticationProperties
                {
                    IsPersistent = false
                });

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