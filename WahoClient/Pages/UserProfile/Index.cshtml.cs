using AutoMapper;
using BusinessObjects.WahoModels;
using DataAccess.AutoMapperConfig;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Drawing.Printing;
using System.Net.Http.Headers;
using ViewModels.CustomerViewModels;
using ViewModels.EmployeeViewModels;
using ViewModels.WahoViewModels;
using Waho.DataService;

namespace WahoClient.Pages.UserProfile
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly HttpClient client = null;
        private string wahoAPIUrl = "";
        private string employeeAPIUrl = "";
        private readonly Author _author;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IndexModel(Author author, IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            wahoAPIUrl = "https://localhost:7019/waho";
            employeeAPIUrl = "https://localhost:7019/waho/Employee";
            _author = author;
            _httpContextAccessor = httpContextAccessor;
        }
        [BindProperty]
        public Employee employee { get; set; }
        [BindProperty]
        public WahoInformation waho { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            if (User.Identity?.IsAuthenticated == false)
            {
                return RedirectToPage("/accessDenied", new { message = "do bạn chưa đăng nhập" });
            }

            // get data from session
            var employeeJson = _httpContextAccessor.HttpContext.Session.GetString("Employee");
            EmployeeVM eSession = JsonConvert.DeserializeObject<EmployeeVM>(employeeJson);
            // find employee by username
            HttpResponseMessage responseEmployee = await client.GetAsync($"{employeeAPIUrl}/username?username={eSession.UserName}&wahoId={eSession.WahoId}");
            if ((int)responseEmployee.StatusCode == 401) await HttpContext.SignOutAsync("CookieAuthentication");

            string strDataEmployee = await responseEmployee.Content.ReadAsStringAsync();
            employee = JsonConvert.DeserializeObject<Employee>(strDataEmployee);

            HttpResponseMessage responsepaging = await client.GetAsync(wahoAPIUrl + "/byNameEmail?email=" + eSession.Email);
            if ((int)responsepaging.StatusCode == 401) await HttpContext.SignOutAsync("CookieAuthentication");

            string strDatapaging = await responsepaging.Content.ReadAsStringAsync();

            if (responsepaging.IsSuccessStatusCode)
            {
                waho = JsonConvert.DeserializeObject<WahoInformation>(strDatapaging);
            }

            return Page();
        }
    }
}