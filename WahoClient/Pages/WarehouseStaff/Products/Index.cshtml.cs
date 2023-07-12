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
using System.Text;


namespace WahoClient.Pages.WarehouseStaff.Products
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient client = null;
        private string productAPIUrl = "";
        private readonly Author _author;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IndexModel(Author author, IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            productAPIUrl = "https://localhost:7019/waho/Products";
            _author = author;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty(SupportsGet = true)]
        public string message { get; set; }
        [BindProperty(SupportsGet = true)]
        public string successMessage { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<Product> Products { get; set; } = default!;
        [BindProperty]
        public Product product { get; set; }

        [BindProperty(SupportsGet = true)]
        public int pageSize { get; set; } = 10;

        [BindProperty(SupportsGet = true)]
        public int pageIndex { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int TotalCount { get; set; } = 0;

        [BindProperty(SupportsGet = true)]
        public string textSearch { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<SubCategory> subCategories { get; set; }

        [BindProperty(SupportsGet = true)]
        public int subCategoryID { get; set; } = -1;

        private string raw_number, raw_subCategorySearch, raw_textSearch;
        [BindProperty(SupportsGet = true)]
        public List<Supplier> suppliers { get; set; }
        //filter by location
        public List<Location> locations { get; set; }
        [BindProperty(SupportsGet = true)]
        public int locationId { get; set; } = -1;
        //filter price range 
        [BindProperty(SupportsGet = true)]
        public int priceTo { get; set; }
        [BindProperty(SupportsGet = true)]
        public int priceFrom { get; set; }
        [BindProperty(SupportsGet = true)]
        public string inventoryLevel { get; set; } = "all";
        public string supplierName { get; set; } = "all";

        public async Task<IActionResult> OnGetAsync()
        {
            //author
            if (!_author.IsAuthor(3))
            {
                return RedirectToPage("/accessDenied", new { message = "Quản lý sản phẩm" });
            }
            //get data from form
            raw_number = HttpContext.Request.Query["pageSize"];
            if (!string.IsNullOrEmpty(raw_number))
            {
                pageSize = int.Parse(raw_number);
            }
            raw_subCategorySearch = HttpContext.Request.Query["subCategory"];
            if (!string.IsNullOrEmpty(raw_subCategorySearch))
            {
                subCategoryID = int.Parse(raw_subCategorySearch);
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
            // location
            string raw_location = HttpContext.Request.Query["location"];
            if (!string.IsNullOrWhiteSpace(raw_location))
            {
                locationId = int.Parse(raw_location);
            }
            // inventory level
            string raw_inventoryLevel = HttpContext.Request.Query["inventoryLevel"];
            if (!string.IsNullOrWhiteSpace(raw_inventoryLevel))
            {
                inventoryLevel = raw_inventoryLevel;
            }
            else
            {
                inventoryLevel = "all";
            }
            // supplier
            string raw_supplier = HttpContext.Request.Query["supplierName"];
            if (!string.IsNullOrWhiteSpace(raw_supplier))
            {
                supplierName = raw_supplier;
            }
            else
            {
                supplierName = "all";
            }
            // get data from session
            string employeeJson = _httpContextAccessor.HttpContext.Session.GetString("Employee");
            EmployeeVM eSession = JsonConvert.DeserializeObject<EmployeeVM>(employeeJson);
            //get subCategoris list by category
            HttpResponseMessage response = await client.GetAsync($"{productAPIUrl}/subcategories?wahoId={eSession.WahoId}");
            string strData = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                 
                 subCategories = JsonConvert.DeserializeObject<List<SubCategory>>(strData);
            }
            //get location list 
            HttpResponseMessage responseLocation = await client.GetAsync($"{productAPIUrl}/location");
            string strDataLocation = await responseLocation.Content.ReadAsStringAsync();
            if (responseLocation.IsSuccessStatusCode)
            {
                locations = JsonConvert.DeserializeObject<List<Location>>(strDataLocation);
            }
            //get product list ===> Count
            HttpResponseMessage responseCount = await client.GetAsync($"{productAPIUrl}/countProduct?textSearch={textSearch}&location={locationId}&priceTo={priceTo}&priceFrom={priceFrom}" +
                $"&inventoryLevel={inventoryLevel}&supplierName={supplierName}&subCategoryID={subCategoryID}&wahoId={eSession.WahoId}");
            string strDataCount = await responseCount.Content.ReadAsStringAsync();
            if (responseCount.IsSuccessStatusCode)
            {
                TotalCount = int.Parse(strDataCount);
            }
            else
            {
                TotalCount = 0;
                Products = new List<Product>();
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
                HttpResponseMessage responseProduct = await client.GetAsync($"{productAPIUrl}/getProductPaging?pageIndex={pageIndex}&pageSize={pageSize}&textSearch={textSearch}" +
                    $"&subCategoryID={subCategoryID}&location={locationId}&priceFrom={priceFrom}&priceTo={priceTo}&inventoryLevel={inventoryLevel}&wahoId={eSession.WahoId}&supplierId={supplierName}");
                string strDataProduct = await responseProduct.Content.ReadAsStringAsync();
                if (responseProduct.IsSuccessStatusCode)
                {
                    Products = JsonConvert.DeserializeObject<List<Product>>(strDataProduct);
                }
            }
            //get suppliers
            HttpResponseMessage responsesuppliers = await client.GetAsync($"{productAPIUrl}/suppliers?wahoId={eSession.WahoId}");
            string strDatasuppliers = await responsesuppliers.Content.ReadAsStringAsync();
            
            if (responsesuppliers.IsSuccessStatusCode)
            {
                suppliers = JsonConvert.DeserializeObject<List<Supplier>>(strDatasuppliers);
            }
            return Page();
        }
    }
}
