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

namespace WahoClient.Pages.Cashier.ReturnOrders
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient client = null;
        private string returnOrderAPIUrl = "";
        private string employeeAPIUrl = "";
        private readonly Author _author;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IndexModel(Author author, IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            returnOrderAPIUrl = "https://localhost:7019/waho/ReturnOrders";
            employeeAPIUrl = "https://localhost:7019/waho/Employee";
            _author = author;
            _httpContextAccessor = httpContextAccessor;
        }
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
        public string employeeID { get; set; } = "all";
        [BindProperty(SupportsGet = true)]
        public string dateFrom { get; set; }

        [BindProperty(SupportsGet = true)]
        public string dateTo { get; set; }
        private string raw_pageSize, raw_textSearch, raw_EmployeeSearch, raw_status, raw_dateFrom, raw_dateTo;
        //list employee
        public List<Employee> employees { get; set; }
        public string status { get; set; } = "all";
        public IList<ReturnOrder> ReturnOrder { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            //author
            if (!_author.IsAuthor(2))
            {
                return RedirectToPage("/accessDenied", new { message = "Thu Ngân" });
            }
            //get data from form
            raw_pageSize = HttpContext.Request.Query["pageSize"];
            if (!string.IsNullOrEmpty(raw_pageSize))
            {
                pageSize = int.Parse(raw_pageSize);
            }
            raw_EmployeeSearch = HttpContext.Request.Query["employeeID"];
            if (!string.IsNullOrEmpty(raw_EmployeeSearch))
            {
                employeeID = raw_EmployeeSearch;
            }
            else
            {
                employeeID = "all";
            }
            raw_textSearch = HttpContext.Request.Query["textSearch"];
            if (!string.IsNullOrWhiteSpace(raw_textSearch))
            {
                textSearch = raw_textSearch.Trim();
            }
            else
            {
                textSearch = "";
            }
            raw_status = HttpContext.Request.Query["status"];
            if (!string.IsNullOrWhiteSpace(raw_status))
            {
                status = raw_status;

            }
            else
            {
                status = "all";
            }

            raw_dateFrom = HttpContext.Request.Query["dateFrom"];
            raw_dateTo = HttpContext.Request.Query["dateTo"];

            if (!string.IsNullOrEmpty(raw_dateFrom))
            {
                dateFrom = raw_dateFrom;
            }
            else
            {
                raw_dateFrom = "";
            }
            if (!string.IsNullOrEmpty(raw_dateTo))
            {
                dateTo = raw_dateTo;
            }
            else
            {
                raw_dateTo = "";
            }
            // get data from session
            string employeeJson = _httpContextAccessor.HttpContext.Session.GetString("Employee");
            EmployeeVM eSession = JsonConvert.DeserializeObject<EmployeeVM>(employeeJson);
            // get list WareHouse Employee
            HttpResponseMessage responseEm = await client.GetAsync($"{employeeAPIUrl}/EmployeesInWahoByRole?role={3}&wahoId={eSession.WahoId}");
            string strDataEm = await responseEm.Content.ReadAsStringAsync();
            if (responseEm.IsSuccessStatusCode)
            {
                employees = JsonConvert.DeserializeObject<List<Employee>>(strDataEm);
            }
            // count return order
            HttpResponseMessage responseCount = await client.GetAsync($"{returnOrderAPIUrl}/count?textSearch={textSearch}&employeeID={employeeID}&status={status}&dateFrom={dateFrom}&dateTo={dateTo}&wahoId={eSession.WahoId}");
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
            successMessage = TempData["SuccessMessage"] as string;
            // get return order paging
            if (TotalCount != 0)
            {
                HttpResponseMessage response = await client.GetAsync($"{returnOrderAPIUrl}/paging?pageIndex={pageIndex}" +
                    $"&pageSize={pageSize}&textSearch={textSearch}&userName={employeeID}&status={status}&raw_dateFrom={dateFrom}&raw_dateTo={dateTo}&wahoId={eSession.WahoId}");
                string strData = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    ReturnOrder = JsonConvert.DeserializeObject<List<ReturnOrder>>(strData);
                }
            }
            return Page();
        }
    }
}
