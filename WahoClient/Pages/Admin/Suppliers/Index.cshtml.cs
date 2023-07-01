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
using Newtonsoft.Json;

namespace WahoClient.Pages.Admin.Suppliers
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient client = null;
        private string supplierAPIUrl = "";
        private readonly Author _author;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IndexModel(Author author, IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            supplierAPIUrl = "https://localhost:7019/waho/Suppliers";
            _author = author;
            _httpContextAccessor = httpContextAccessor;
        }
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
        private string raw_pageSize, raw_textSearch;
        [BindProperty(SupportsGet = true)]
        public IList<Supplier> Supplier { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            //author
            if (!_author.IsAuthor(1))
            {
                return RedirectToPage("/accessDenied", new { message = "Trình quản lý của Admin" });
            }

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
            // get data from session
            var employeeJson = _httpContextAccessor.HttpContext.Session.GetString("Employee");
            EmployeeVM employeeVM = JsonConvert.DeserializeObject<EmployeeVM>(employeeJson);
            // count total supplier
            
            HttpResponseMessage response = await client.GetAsync(supplierAPIUrl + "/countPagingSupplier?textSearch=" + textSearch + "&wahoId=" + employeeVM.WahoId);
            string strData = await response.Content.ReadAsStringAsync();
            TotalCount = int.Parse(strData);
            message = TempData["message"] as string;
            successMessage = TempData["successMessage"] as string;
            // get supplier paging
            
            HttpResponseMessage responsepaging = await client.GetAsync(supplierAPIUrl + "/getSupplierPaging?pageIndex=" + pageIndex + "&pageSize=" + pageSize + "&textSearch=" + textSearch + "&wahoId=" + employeeVM.WahoId);
            string strDatapaging = await responsepaging.Content.ReadAsStringAsync();

            if (responsepaging.IsSuccessStatusCode)
            {
                Supplier = JsonConvert.DeserializeObject<List<Supplier>>(strDatapaging);
            }
            return Page();
        }
    }
}
