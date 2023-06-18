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
using ViewModels.InventorySheetViewModels;
using System.Text;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using DataAccess.AutoMapperConfig;
using ViewModels.ProductViewModels;

namespace WahoClient.Pages.WarehouseStaff.InventorySheetManager
{
    public class DetailsModel : PageModel
    {
        private readonly HttpClient client = null;
        private string inventoryAPIUrl = "";
        private string productAPIUrl = "";
        private readonly Author _author;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static readonly IMapper _mapperDetail = InventorySheetDetailMapper.ConfigureMToVM();
        private static readonly IMapper _mapper = ProductConfigMapper.ConfigureMToVM();
        public DetailsModel(Author author, IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            inventoryAPIUrl = "https://localhost:7019/waho/InventorySheets";
            productAPIUrl = "https://localhost:7019/waho/Products";
            _author = author;
            _httpContextAccessor = httpContextAccessor;
        }
        //message
        public string message { get; set; }
        public string successMessage { get; set; }
        // paging
        [BindProperty(SupportsGet = true)]
        public int pageSize { get; set; } = 10;

        [BindProperty(SupportsGet = true)]
        public int pageIndex { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int TotalCount { get; set; } = 0;

        private string raw_pageSize;
        public InventorySheet _inventorySheet { get; set; } = default!;
        public List<InventorySheetDetail> inventorySheetDetails { get; set; }
        public List<InventorySheetDetail> inventorySheetDetailAll { get; set; }
        [BindProperty(SupportsGet = true)]
        public int _inventorySheetID { get; set; }

        public async Task<IActionResult> OnGetAsync(int inventorySheetID)
        {
            //author
            if (!_author.IsAuthor(3))
            {
                return RedirectToPage("/accessDenied", new { message = "Quản lý sản phẩm" });
            }
            //get data from form
            raw_pageSize = HttpContext.Request.Query["pageSize"];
            if (inventorySheetID != 0)
            {
                _inventorySheetID = inventorySheetID;
            }
            if (HttpContext.Request.HasFormContentType == true)
            {

                if (!string.IsNullOrEmpty(HttpContext.Request.Form["inventorySheetID"]))
                {
                    _inventorySheetID = Int32.Parse(HttpContext.Request.Form["inventorySheetID"]);
                }
            }
            if (!string.IsNullOrEmpty(raw_pageSize))
            {
                pageSize = int.Parse(raw_pageSize);
            }

            if (inventorySheetID == null)
            {
                //message
                message = "Không tìm thấy mã phiếu kiểm kho";
                TempData["message"] = message;
                return RedirectToPage("./Index");
            }
            // get data from session
            string employeeJson = _httpContextAccessor.HttpContext.Session.GetString("Employee");
            EmployeeVM eSession = JsonConvert.DeserializeObject<EmployeeVM>(employeeJson);

            // get inventory sheet by id
            HttpResponseMessage responseInventory = await client.GetAsync($"{inventoryAPIUrl}/getInventorySheetById?inventorySheetId={inventorySheetID}");
            string strDataInventory = await responseInventory.Content.ReadAsStringAsync();
            if (responseInventory.IsSuccessStatusCode)
            {
                _inventorySheet = JsonConvert.DeserializeObject<InventorySheet>(strDataInventory);
            }
            // get all inventory details of inventory
            HttpResponseMessage responseInventoryDetailAll = await client.GetAsync($"{inventoryAPIUrl}/getInventoryDetails?inventorySheetId={_inventorySheetID}");
            string strDataInventoryDetailAl = await responseInventoryDetailAll.Content.ReadAsStringAsync();
            List<InventorySheetDetail> _inventorySheetDetails = new List<InventorySheetDetail>();
            if (responseInventoryDetailAll.IsSuccessStatusCode)
            {
                _inventorySheetDetails = JsonConvert.DeserializeObject<List<InventorySheetDetail>>(strDataInventoryDetailAl);
            }

            inventorySheetDetailAll = _inventorySheetDetails.ToList();
            TotalCount = _inventorySheetDetails.Count;
            // get list inventory detail paging
            if (_inventorySheetDetails != null)
            {
                // paging inventory sheet details
                HttpResponseMessage responseInventoryDetailPaging = await client.GetAsync($"{inventoryAPIUrl}/getInventoryDetailsPaging?pageIndex={pageIndex}&pageSize={pageSize}&InventoryId={inventorySheetID}");
                string strDataInventoryDetailPaging = await responseInventoryDetailPaging.Content.ReadAsStringAsync();
                if (responseInventoryDetailPaging.IsSuccessStatusCode)
                {
                    inventorySheetDetails = JsonConvert.DeserializeObject<List<InventorySheetDetail>>(strDataInventoryDetailPaging);
                    return Page();
                }
            }
            //message
            message = "Không tìm thấy mã phiếu kiểm kho";
            TempData["message"] = message;
            return RedirectToPage("./Index");
        }
        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine("debug");
            InventorySheetVM _inventorySheetUpdate = new InventorySheetVM();
            var req = HttpContext.Request;
            //get data form form submit 
            string raw_EmployeeID = req.Form["employeeID"];
            string raw_inventorySheetID = req.Form["inventorySheetID"];
            string raw_date = req.Form["date"];
            string raw_description = req.Form["description"];
            _inventorySheetUpdate.InventorySheetId = Int32.Parse(raw_inventorySheetID);
            _inventorySheetUpdate.UserName = raw_EmployeeID;
            if (string.IsNullOrWhiteSpace(raw_date))
            {
                //message
                message = "Ngày kiểm kho không được để trống";
                TempData["message"] = message;
                return RedirectToPage("./Index");
            }
            _inventorySheetUpdate.Date = DateTime.Parse(raw_date);
            _inventorySheetUpdate.Description = raw_description;
            _inventorySheetUpdate.Active = true;
            // get data from session
            string employeeJson = _httpContextAccessor.HttpContext.Session.GetString("Employee");
            EmployeeVM eSession = JsonConvert.DeserializeObject<EmployeeVM>(employeeJson);
            _inventorySheetUpdate.WahoId = eSession.WahoId;
            // update inventory sheet
            var json = JsonConvert.SerializeObject(_inventorySheetUpdate);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(inventoryAPIUrl, content);
            if (response.IsSuccessStatusCode)
            {
                //success message
                successMessage = "Chỉnh sửa thông tin phiếu thành công";
                TempData["successMessage"] = successMessage;
                return RedirectToPage("./Index");
            }
            //fail message
            message = "Chỉnh sửa thông tin phiếu thất bại";
            TempData["message"] = message;
            return RedirectToPage("./Index");

        }
        public async Task<IActionResult> OnPostUpdateDetail()
        {
            var req = HttpContext.Request;
            //get data form form submit 
            string currennumbers = req.Form["CurNwareHouse"];
            int inventorySheetID = Int32.Parse(req.Form["inventorySheetID"]);
            string productIDs = req.Form["productIDUpdate"];
            string productQuantity = req.Form["Quantity"];
            string[] currennumbersList = currennumbers.Split(',');
            string[] productIDsList = productIDs.Split(',');
            string[] productQuantityList = productQuantity.Split(',');
            int count = 0;
            foreach (var i in currennumbersList)
            {
                int currentNumber = Int32.Parse(i);
                int productID = Int32.Parse(productIDsList[count]);
                int quantity = Int32.Parse(productQuantityList[count]);
                // update product information in inventorySheet
                InventorySheetDetailVM _inventorySheetDetail = new InventorySheetDetailVM();

                // find inventory sheet detail by product id and inventory sheet id
                HttpResponseMessage responseInventoryDe = await client.GetAsync($"{inventoryAPIUrl}/getInventoryDetails-ByProId-InvenId?productId={productID}&inventorySheetId={inventorySheetID}");
                string strDataInventoryDe = await responseInventoryDe.Content.ReadAsStringAsync();
                if (responseInventoryDe.IsSuccessStatusCode)
                {
                       InventorySheetDetail inDetail = JsonConvert.DeserializeObject<InventorySheetDetail>(strDataInventoryDe);
                     _inventorySheetDetail = _mapperDetail.Map<InventorySheetDetailVM>(inDetail);
                }
                _inventorySheetDetail.CurNwareHouse = currentNumber;
                // update inventory sheet detail
                var json = JsonConvert.SerializeObject(_inventorySheetDetail);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"{inventoryAPIUrl}/detail", content);
                if (response.IsSuccessStatusCode)
                {
                    // update information of product in product list
                    ProductViewModel product = new ProductViewModel();
                    HttpResponseMessage responsePro = await client.GetAsync($"{productAPIUrl}/productId?productId={productID}");
                    string strDataPro = await responsePro.Content.ReadAsStringAsync();
                    if (responsePro.IsSuccessStatusCode)
                    {
                        Product _product = JsonConvert.DeserializeObject<Product>(strDataPro);
                        product = _mapper.Map<ProductViewModel>(_product);
                    }
                    //product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productID);
                    product.Quantity = quantity;
                    var jsonPro = JsonConvert.SerializeObject(product);
                    var contentPro = new StringContent(jsonPro, Encoding.UTF8, "application/json");
                    var responseProductPut = await client.PutAsync(productAPIUrl, contentPro);
                    if (response.IsSuccessStatusCode)
                    {
                        //message
                        successMessage = "lưu thông tin sản phẩm thành công";
                        TempData["successMessage"] = successMessage;
                    }
                    //message
                    TempData["success"] = "lưu thông tin sản phẩm thất bại";
                    count++;
                }
                
            }

            try
            {
                //success message
                successMessage = "Lưu thông tin sản phẩm kiểm tra thành công";
                TempData["SuccessMessage"] = successMessage;
                return RedirectToPage("./Details", new { inventorySheetID });
            }
            catch (DbUpdateConcurrencyException)
            {
                return Page();
            }
        }
    }
}
