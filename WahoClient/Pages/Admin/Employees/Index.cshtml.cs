using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.WahoModels;
using Waho.DataService;
using System.Net.Http.Headers;
using ViewModels.EmployeeViewModels;
//using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Authentication;

namespace WahoClient.Pages.Admin.Employees
{
    [Authorize(Roles = "1")]

    public class IndexModel : PageModel
    {
        private readonly HttpClient client = null;
        private string employeeAPIUrl = "";
        private readonly Author _author;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //message
        [BindProperty(SupportsGet = true)]
        public string message { get; set; }
        [BindProperty(SupportsGet = true)]
        public string successMessage { get; set; }
        // paging
        [BindProperty(SupportsGet = true)]
        public int pageSize { get; set; } = 10;

        [BindProperty(SupportsGet = true)]
        public int pageIndex { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int TotalCount { get; set; } = 0;

        [BindProperty(SupportsGet = true)]
        public string textSearch { get; set; }
        [BindProperty(SupportsGet = true)]
        public string status { get; set; } = "all";
        private string raw_pageSize, raw_textSearch;
        public IndexModel( Author author, IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            employeeAPIUrl = "https://localhost:7019/waho/Employee";
            _author = author;
            _httpContextAccessor = httpContextAccessor;
        }
        [BindProperty(SupportsGet = true)]
        public IList<Employee> Employee { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string title { get; set; } = "all";

        public async Task<IActionResult> OnGetAsync()
        {
            //author
            if (User.Identity?.IsAuthenticated == false)
            {
                return RedirectToPage("/accessDenied", new { message = "do bạn chưa đăng nhập" });
            }
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Request.Cookies["AccessToken"]);
            //get data from form
            raw_pageSize = HttpContext.Request.Query["pageSize"];
            if (!string.IsNullOrEmpty(raw_pageSize))
            {
                pageSize = int.Parse(raw_pageSize);
            }
            raw_textSearch = HttpContext.Request.Query["textSearch"];
            if (!string.IsNullOrWhiteSpace(raw_textSearch))
            {
                textSearch = raw_textSearch.Trim().ToLower();
            }
            else
            {
                textSearch = "";
            }
            string raw_title = HttpContext.Request.Query["title"];
            if (!string.IsNullOrWhiteSpace(raw_title))
            {
                title = raw_title.Trim();
            }
            else
            {
                title = "all";
            }
            // get data from session
            var employeeJson = _httpContextAccessor.HttpContext.Session.GetString("Employee");
            EmployeeVM employeeVM = JsonConvert.DeserializeObject<EmployeeVM>(employeeJson);
            //api total count
            HttpResponseMessage response = await client.GetAsync(employeeAPIUrl + "/countPagingEmployee?textSearch=" + textSearch + "&status=" + status + "&title="+ title + "&wahoId=" + employeeVM.WahoId);
            if ((int)response.StatusCode == 401) await HttpContext.SignOutAsync("CookieAuthentication");

            string strData = await response.Content.ReadAsStringAsync();
            TotalCount = int.Parse(strData);

            message = TempData["message"] as string;
            successMessage = TempData["successMessage"] as string;
            // api paging
            HttpResponseMessage responsepaging = await client.GetAsync(employeeAPIUrl + "/getEmployeePaging?pageIndex="+ pageIndex + "&pageSize="+ pageSize + "&textSearch=" + textSearch + "&status=" + status + "&title=" + title + "&wahoId=" + employeeVM.WahoId);
            if ((int)responsepaging.StatusCode == 401) await HttpContext.SignOutAsync("CookieAuthentication");

            string strDatapaging = await responsepaging.Content.ReadAsStringAsync();
           
            if (responsepaging.IsSuccessStatusCode)
            {
                Employee = JsonConvert.DeserializeObject<List<Employee>>(strDatapaging);
            }
            return Page();
        }
    }
}
