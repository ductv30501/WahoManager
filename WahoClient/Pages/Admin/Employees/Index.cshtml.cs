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
using System.Text.Json;

namespace WahoClient.Pages.Admin.Employees
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient client = null;
        private string employeeAPIUrl = "";
        private readonly Author _author;
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
        public IndexModel( Author author)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            employeeAPIUrl = "https://localhost:7019/waho/Employee";
            _author = author;
        }
        [BindProperty(SupportsGet = true)]
        public IList<Employee> Employee { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string title { get; set; } = "all";

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
            string raw_title = HttpContext.Request.Query["title"];
            if (!string.IsNullOrWhiteSpace(raw_title))
            {
                title = raw_title.Trim();
            }
            else
            {
                title = "all";
            }
            HttpResponseMessage response = await client.GetAsync(employeeAPIUrl + "/countPagingEmployee?textSearch=" + textSearch + "&status=" + status + "&title="+ title);
            string strData = await response.Content.ReadAsStringAsync();
            TotalCount = int.Parse(strData);

            message = TempData["message"] as string;
            successMessage = TempData["successMessage"] as string;
            HttpResponseMessage responsepaging = await client.GetAsync(employeeAPIUrl + "/getEmployeePaging?pageIndex="+ pageIndex + "&pageSize="+ pageSize + "&textSearch=" + textSearch + "&status=" + status + "&title=" + title);
            string strDatapaging = await responsepaging.Content.ReadAsStringAsync();
            var optionspaging = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            Employee = JsonSerializer.Deserialize<List<Employee>>(strDatapaging,optionspaging);
            return Page();
        }
    }
}
