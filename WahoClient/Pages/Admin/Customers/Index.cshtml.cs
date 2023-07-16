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
using Newtonsoft.Json;
using ViewModels.EmployeeViewModels;

namespace WahoClient.Pages.Admin.Customers
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient client = null;
        private string customerAPIUrl = "";
        private readonly Author _author;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IndexModel(Author author, IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            customerAPIUrl = "https://localhost:7019/waho/Customer";
            _author = author;
            _httpContextAccessor = httpContextAccessor;
        }
        [BindProperty(SupportsGet = true)]
        public string message { get; set; }
        [BindProperty(SupportsGet = true)]
        public string successMessage { get; set; }
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

        [BindProperty(SupportsGet = true)]
        public string typeCustomer { get; set; } = "all";

        [BindProperty(SupportsGet = true)]
        public string dateFrom { get; set; }

        [BindProperty(SupportsGet = true)]
        public string dateTo { get; set; }

        private string raw_number, raw_textSearch;

        public IList<Customer> Customers { get; set; } = default!;
        public EmployeeVM employeeVM { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
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

            // get data from session
            var employeeJson = _httpContextAccessor.HttpContext.Session.GetString("Employee");
            employeeVM = JsonConvert.DeserializeObject<EmployeeVM>(employeeJson);
            //api total count
            HttpResponseMessage response = await client.GetAsync(customerAPIUrl + "/count?textSearch=" + textSearch + "&status=" + status + "&typeCustomer=" + typeCustomer + "&wahoId=" + employeeVM.WahoId);
            string strData = await response.Content.ReadAsStringAsync();
            TotalCount = int.Parse(strData);

            message = TempData["message"] as string;
            successMessage = TempData["successMessage"] as string;
            // api paging
            HttpResponseMessage responsepaging = await client.GetAsync(customerAPIUrl + "/get s?pageIndex=" + pageIndex + "&pageSize=" + pageSize + "&textSearch=" + textSearch + "&status=" + status + "&typeCustomer=" + typeCustomer + "&wahoId=" + employeeVM.WahoId);
            string strDatapaging = await responsepaging.Content.ReadAsStringAsync();

            if (responsepaging.IsSuccessStatusCode)
            {
                Customers = JsonConvert.DeserializeObject<List<Customer>>(strDatapaging);
            }
            return Page();
        }
    }
}
