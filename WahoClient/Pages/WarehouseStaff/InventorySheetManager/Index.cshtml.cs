using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.WahoModels;
using System.Net.Http.Headers;
using Waho.DataService;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ViewModels.EmployeeViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace WahoClient.Pages.WarehouseStaff.InventorySheetManager
{
    [Authorize(Roles ="1,3")]
    public class IndexModel : PageModel
    {
        private readonly HttpClient client = null;
        private string inventoryAPIUrl = "";
        private string employeeAPIUrl = "";
        private readonly Author _author;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IndexModel(Author author, IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            inventoryAPIUrl = "https://localhost:7019/waho/InventorySheets";
            employeeAPIUrl = "https://localhost:7019/waho/Employee";
            _author = author;
            _httpContextAccessor = httpContextAccessor;
        }
        [BindProperty(SupportsGet = true)]
        public IList<InventorySheet> InventorySheet { get;set; } = default!;
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
        public string textSearch { get; set; } = "";
        [BindProperty(SupportsGet = true)]
        public string employeeID { get; set; } = "all";
        [BindProperty(SupportsGet = true)]
        public string dateFrom { get; set; }

        [BindProperty(SupportsGet = true)]
        public string dateTo { get; set; }
        private string raw_pageSize, raw_EmployeeSearch, raw_textSearch, raw_dateFrom, raw_dateTo;
        //list employee
        [BindProperty(SupportsGet = true)]
        public List<Employee> employees { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
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
            raw_EmployeeSearch = HttpContext.Request.Query["employeeID"];
            if (!string.IsNullOrEmpty(raw_EmployeeSearch))
            {
                employeeID = raw_EmployeeSearch.Trim().ToLower();
            }
            else
            {
                employeeID = "all";
            }
            raw_textSearch = HttpContext.Request.Query["textSearch"];
            if (!string.IsNullOrWhiteSpace(raw_textSearch))
            {
                textSearch = raw_textSearch;
            }
            else
            {
                textSearch = "";
            }
            raw_dateFrom = HttpContext.Request.Query["dateFrom"];
            raw_dateTo = HttpContext.Request.Query["dateTo"];

            dateFrom = raw_dateFrom;
            
            dateTo = raw_dateTo;
            // get data from session
            string employeeJson = _httpContextAccessor.HttpContext.Session.GetString("Employee");
            EmployeeVM eSession = JsonConvert.DeserializeObject<EmployeeVM>(employeeJson);
            // get list WareHouse Employee
            HttpResponseMessage responseEmployee = await client.GetAsync($"{employeeAPIUrl}/EmployeesInWaho?wahoId={eSession.WahoId}");
            if ((int)responseEmployee.StatusCode == 401) await HttpContext.SignOutAsync("CookieAuthentication");

            string strDataEmployee = await responseEmployee.Content.ReadAsStringAsync();
            if (responseEmployee.IsSuccessStatusCode)
            {
                employees = JsonConvert.DeserializeObject<List<Employee>>(strDataEmployee);
            }
            //count inventory sheet in list 
            HttpResponseMessage responseCount = await client.GetAsync($"{inventoryAPIUrl}/countInventories?textSearch={textSearch}&employeeID={employeeID}&raw_dateFrom={dateFrom}&raw_dateTo={dateTo}&wahoId={eSession.WahoId}");
            if ((int)responseCount.StatusCode == 401) await HttpContext.SignOutAsync("CookieAuthentication");

            string strDataCount = await responseCount.Content.ReadAsStringAsync();
            if (responseCount.IsSuccessStatusCode)
            {
                TotalCount = int.Parse(strDataCount);
            }
            else
            {
                TotalCount = 0;
            }
            //gán lại giá trị pageIndex khi page index vợt quá pageSize khi filter
            if ((pageIndex - 1) > (TotalCount / pageSize))
            {
                pageIndex = 1;
            }
            message = TempData["message"] as string;
            successMessage = TempData["successMessage"] as string;
            if (TotalCount != 0)
            {
                HttpResponseMessage responseInventory = await client.GetAsync($"{inventoryAPIUrl}/getInventoryPaging?pageIndex={pageIndex}&pageSize={pageSize}&textSearch={textSearch}&employeeID={employeeID}&raw_dateFrom={dateFrom}&raw_dateTo={dateTo}&wahoId={eSession.WahoId}");
                if ((int)responseInventory.StatusCode == 401) await HttpContext.SignOutAsync("CookieAuthentication");

                string strDataInventory = await responseInventory.Content.ReadAsStringAsync();
                if (responseInventory.IsSuccessStatusCode)
                {
                    InventorySheet = JsonConvert.DeserializeObject<List<InventorySheet>>(strDataInventory);
                }
            }
            return Page();
        }
    }
}
