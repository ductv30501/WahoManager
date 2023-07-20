using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjects.WahoModels;
using Waho.DataService;
using System.Net.Http.Headers;
using ViewModels.ProductViewModels;
using Newtonsoft.Json;
using System.Text;
using ViewModels.EmployeeViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Authentication;

namespace WahoClient.Pages.WarehouseStaff.Products
{
    [Authorize(Roles = "1,3")]

    public class CreateModel : PageModel
    {
        private readonly HttpClient client = null;
        private string productAPIUrl = "";
        private readonly Author _author;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateModel(Author author, IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            productAPIUrl = "https://localhost:7019/waho/Products";
            _author = author;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult OnGet()
        {
            //if (!_author.IsAuthor(3))
            //{
            //    return RedirectToPage("/accessDenied", new { message = "Quản lý sản phẩm" });
            //}
            if (User.Identity?.IsAuthenticated == false)
            {
                return RedirectToPage("/accessDenied", new { message = "do bạn chưa đăng nhập" });
            }
            return Page();
        }

        [BindProperty]
        public ProductViewModel Product { get; set; } = default!;
        public string message { get; set; }
        public string successMessage { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Request.Cookies["AccessToken"]);

            var req = HttpContext.Request;
            //get data form form submit 
            string raw_productName = req.Form["productName"];
            string raw_importPrice = req.Form["importPrice"];
            string raw_unitPrice = req.Form["unitPrice"];
            string raw_trademark = req.Form["trademark"];
            string raw_quantity = req.Form["quantity"];
            string raw_weight = req.Form["weight"];
            string raw_location = req.Form["location"];
            string raw_unit = req.Form["unit"];
            string raw_inventoryLevelMin = req.Form["inventoryLevelMin"];
            string raw_inventoryLevelMax = req.Form["inventoryLevelMax"];
            string raw_description = req.Form["description"];
            string raw_subCategory = req.Form["subCategory"];
            string raw_supplierID = req.Form["supplierID"];

            Product.ProductName = raw_productName;
            if (string.IsNullOrWhiteSpace(raw_importPrice))
            {
                int import_Price = Int32.Parse(raw_importPrice);
                if (import_Price > 0)
                {
                    Product.ImportPrice = import_Price;
                }
                else
                {
                    // message
                    message = "Bạn cần nhập giá lớn hơn 0 đồng";
                    TempData["message"] = message;
                    return RedirectToPage("./Index");
                }
            }
            if (string.IsNullOrWhiteSpace(raw_unitPrice))
            {
                int unitPrice = Int32.Parse(raw_unitPrice);
                if (unitPrice > 0)
                {
                    Product.UnitPrice = unitPrice;
                }
                else
                {
                    // messagse
                    message = "bạn cần nhập đơn vị giá lớn hơn 0 đồng";
                    TempData["message"] = message;
                    return RedirectToPage("./Index");
                }
            }
            Product.Trademark = raw_trademark;
            if (string.IsNullOrWhiteSpace(raw_quantity))
            {
                int quantity = Int32.Parse(raw_quantity);
                if (quantity > 0)
                {
                    Product.Quantity = quantity;
                }
                else
                {
                    // messagse
                    message = "bạn cần nhập số lượng lớn hơn 0";
                    TempData["message"] = message;
                    return RedirectToPage("./Index");
                }
            }
            if (string.IsNullOrWhiteSpace(raw_weight))
            {
                int weight = Int32.Parse(raw_weight);
                if (weight > 0)
                {
                    Product.Weight = weight;
                }
                else
                {
                    // messagse
                    message = "bạn cần nhập trọng lượng lớn hơn 0 đồng";
                    TempData["message"] = message;
                    return RedirectToPage("./Index");
                }
            }
            Product.LocationId = int.Parse(raw_location);
            Product.Description = raw_description;
            Product.Unit = raw_unit;
            if (string.IsNullOrWhiteSpace(raw_inventoryLevelMin))
            {
                int inventoryLevelMin = Int32.Parse(raw_inventoryLevelMin);
                if (inventoryLevelMin > 0)
                {
                    Product.InventoryLevelMin = inventoryLevelMin;
                }
                else
                {
                    // messagse
                    message = "bạn cần nhập số lượng tồn nhỏ nhất là 1";
                    TempData["message"] = message;
                    return RedirectToPage("./Index");
                }
            }
            if (string.IsNullOrWhiteSpace(raw_inventoryLevelMax))
            {
                int inventoryLevelMax = Int32.Parse(raw_inventoryLevelMax);
                if (inventoryLevelMax > 0)
                {
                    Product.InventoryLevelMax = inventoryLevelMax;
                }
                else
                {
                    // messagse
                    message = "bạn cần nhập số lượng tồn nhỏ lớn nhất phải lớn hơn 0 ";
                    TempData["message"] = message;
                    return RedirectToPage("./Index");
                }
            }
            int subCategoryID = Int32.Parse(raw_subCategory);
            Product.SubCategoryId = subCategoryID;
            int supplierID = Int32.Parse(raw_supplierID);
            Product.SupplierId = supplierID;

            // process for product have date
            string raw_dateOfManufacture = req.Form["dateOfManufacture"];
            string raw_expiry = req.Form["expiry"];
            if (string.IsNullOrEmpty(raw_expiry) || string.IsNullOrEmpty(raw_dateOfManufacture))
            {
                Product.HaveDate = false;
            }
            else
            {
                Product.HaveDate = true;
                DateTime dateOfManufacture = DateTime.Parse(raw_dateOfManufacture);
                DateTime expiry = DateTime.Parse(raw_expiry);
                DateTime dateNow = DateTime.Now;
                if (dateOfManufacture.CompareTo(dateNow) > 0 || expiry.CompareTo(dateOfManufacture) < 0 || expiry.CompareTo(dateNow) < 0)
                {
                    //message
                    message = "bạn không được nhập HSD nhỏ hơn NSX hoặc NXS,HSD nhỏ hơn thời gian hiện tại";
                    TempData["message"] = message;
                    return RedirectToPage("./Index");
                }
                else
                {
                    Product.DateOfManufacture = dateOfManufacture;
                    Product.Expiry = expiry;
                }
            }
            // get data from session
            var employeeJson = _httpContextAccessor.HttpContext.Session.GetString("Employee");
            EmployeeVM Esession = JsonConvert.DeserializeObject<EmployeeVM>(employeeJson);
            Product.WahoId = Esession.WahoId;
            Product.Active = true;
            //add product
            var json = JsonConvert.SerializeObject(Product);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(productAPIUrl, content);
            if ((int)response.StatusCode == 401) await HttpContext.SignOutAsync("CookieAuthentication");

            if (response.IsSuccessStatusCode)
            {
                successMessage = "Đã thêm thành công 1 sản phẩm";
                TempData["successMessage"] = successMessage;
                return RedirectToPage("./Index");
            }
            message = "Thêm sản phẩm mới thất bại";
            TempData["message"] = message;
            return RedirectToPage("./Index");
        }
    }
}
