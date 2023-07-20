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
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Authentication;

namespace WahoClient.Pages.Cashier.Orders
{
    [Authorize(Roles = "1,2")]

    public class IndexModel : PageModel
    {
        private readonly HttpClient client = null;
        private string orderAPIUrl = "";
        private readonly Author _author;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IndexModel(Author author, IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            orderAPIUrl = "https://localhost:7019/waho/Orders";
            _author = author;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty(SupportsGet = true)]
        public int pageSize { get; set; } = 10;

        [BindProperty(SupportsGet = true)]
        public int pageIndex { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int TotalCount { get; set; } = 0;
        [BindProperty(SupportsGet = true)]
        public string textSearch { get; set; }

        [BindProperty(SupportsGet = true)]
        public string dateFrom { get; set; }

        [BindProperty(SupportsGet = true)]
        public string dateTo { get; set; }

        [BindProperty(SupportsGet = true)]
        public string estDateFrom { get; set; }

        [BindProperty(SupportsGet = true)]
        public string estDateTo { get; set; }

        [BindProperty(SupportsGet = true)]
        public string status { get; set; } = "all";

        [BindProperty(SupportsGet = true)]
        public string active { get; set; } = "all";

        private string raw_number, raw_textSearch;

        public IList<Oder> Orders { get; set; } = default!;

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
            raw_number = HttpContext.Request.Query["pageSize"];
            if (!string.IsNullOrEmpty(raw_number))
            {
                pageSize = int.Parse(raw_number);
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

            dateFrom = HttpContext.Request.Query["dateFrom"];
            dateTo = HttpContext.Request.Query["dateTo"];
            estDateFrom = HttpContext.Request.Query["estDateFrom"];
            estDateTo = HttpContext.Request.Query["estDateTo"];
            // get data from session
            string employeeJson = _httpContextAccessor.HttpContext.Session.GetString("Employee");
            EmployeeVM eSession = JsonConvert.DeserializeObject<EmployeeVM>(employeeJson);
            // count order list
            HttpResponseMessage responseCount = await client.GetAsync($"{orderAPIUrl}/count?textSearch={textSearch}" +
                $"&active={active}&status={status}&dateTo={dateTo}&dateFrom={dateFrom}&estDateTo={estDateTo}&estDateFrom={estDateFrom}&wahoId={eSession.WahoId}");
            if ((int)responseCount.StatusCode == 401) return RedirectToPage("/accessDenied");

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

            // get order paging
            if (TotalCount != 0)
            {
                HttpResponseMessage responseOrder = await client.GetAsync($"{orderAPIUrl}/paging?pageIndex={pageIndex}" +
                    $"&pageSize={pageSize}&textSearch={textSearch}&status={status}&dateFrom={dateFrom}&estDateFrom={estDateFrom}&estDateTo={estDateTo}&dateTo={dateTo}&active={active}&wahoId={eSession.WahoId}");
            if ((int)responseOrder.StatusCode == 401) return RedirectToPage("/accessDenied");

                string strDataOrder = await responseOrder.Content.ReadAsStringAsync();
                if (responseOrder.IsSuccessStatusCode)
                {
                    Orders = JsonConvert.DeserializeObject<List<Oder>>(strDataOrder);
                }
            }

            return Page();
        }
    }
}
