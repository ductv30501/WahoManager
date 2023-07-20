using BusinessObjects.WahoModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using ViewModels.EmployeeViewModels;
using ViewModels.ProductViewModels;
using Waho.DataService;

namespace WahoClient.Pages.WarehouseStaff.Products
{
    [Authorize(Roles = "1,3")]

    public class UploadFileExcelModel : PageModel
    {
        private readonly IFileProvider _fileProvider;
        private readonly HttpClient client = null;
        private string productAPIUrl = "";
        private readonly Author _author;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public string message { get; set; }
        public string successMessage { get; set; }
        public UploadFileExcelModel(IFileProvider fileProvider, Author author, IHttpContextAccessor httpContextAccessor)
        {
            _fileProvider = fileProvider;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            productAPIUrl = "https://localhost:7019/waho/Products";
            _author = author;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public List<Product> Products { get; set; }
        [BindProperty]
        public string ExcelFile { get; set; }
        public IActionResult OnGetAsync()
        {
            //author
            //if (!_author.IsAuthor(3))
            //{
            //    return RedirectToPage("/accessDenied", new { message = "Quản lý sản phẩm" });
            //}

            if (User.Identity?.IsAuthenticated == false)
            {
                return RedirectToPage("/accessDenied", new { message = "do bạn chưa đăng nhập" });
            }
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Request.Cookies["AccessToken"]);

            // Lấy thông tin file từ IFileProvider
            IFileInfo fileInfo = _fileProvider.GetFileInfo("Products.xlsx");
            if (fileInfo.Exists)
            {
                // Đọc nội dung file và trả về file download
                var stream = new MemoryStream();
                using (var fileStream = fileInfo.CreateReadStream())
                {
                    fileStream.CopyTo(stream);
                }
                stream.Position = 0;
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Products.xlsx");
            }
            else
            {
                // messagse
                message = "file không tồn tại";
                TempData["message"] = message;
                return RedirectToPage("./Index");
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Request.Cookies["AccessToken"]);

            var req = HttpContext.Request;
            if (string.IsNullOrEmpty(req.Form["ExcelFile"]))
            {
                // messagse
                message = "đường dẫn file trống, null";
                TempData["message"] = message;
                return RedirectToPage("./Index");
            }
            else
            {

                IFileInfo fileInfo = _fileProvider.GetFileInfo(req.Form["ExcelFile"]);
                ExcelFile = fileInfo.PhysicalPath;
            }
            if (ExcelFile == null || ExcelFile.Length == 0)
            {
                // messagse
                message = "hãy chọn file trước khi gửi";
                TempData["message"] = message;
                return RedirectToPage("./Index");
            }
            else
            {
                try
                {
                    List<ProductViewModel> products = ReadExcel(ExcelFile);
                    if (products == null)
                    {
                        // messagse
                        message = "trong file không có sản phẩm nào";
                        TempData["message"] = message;
                        return RedirectToPage("./Index");
                    }
                    else
                    {
                        // add waho id 
                        // get data from session
                        string employeeJson = _httpContextAccessor.HttpContext.Session.GetString("Employee");
                        EmployeeVM eSession = JsonConvert.DeserializeObject<EmployeeVM>(employeeJson);
                        foreach (var p in products)
                        {
                            p.WahoId = eSession.WahoId;
                        }
                        // add list product to db
                        var json = JsonConvert.SerializeObject(products);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        var response = await client.PostAsync($"{productAPIUrl}/addproducts", content);
                        if ((int)response.StatusCode == 401) return RedirectToPage("/accessDenied");
                        if (response.IsSuccessStatusCode)
                        {
                            successMessage = $"{products.Count} sản phẩm được thêm vào thành công";
                            TempData["successMessage"] = successMessage;
                            return RedirectToPage("./Index");
                        }
                        //await _context.BulkInsertAsync(products); 
                        message = "kiểm tra lại file trước khi tải lên";
                        TempData["message"] = message;
                        return RedirectToPage("./Index");
                    }
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    TempData["message"] = "Bạn tải lên sai file, vui lòng nhập đúng theo mẫu";
                    return RedirectToPage("./Index");
                }
            }
            //return Page();


        }
        private List<ProductViewModel> ReadExcel(string path)
        {
            using var package = new ExcelPackage(new FileInfo(path));
            var worksheet = package.Workbook.Worksheets[0];
            var products = new List<ProductViewModel>();
            for (int row = 2; row <= worksheet.Dimension.Rows; row++)
            {
                var product = new ProductViewModel
                {
                    ProductName = worksheet.Cells[row, 1].GetValue<string>(),
                    ImportPrice = worksheet.Cells[row, 2].GetValue<int>(),
                    UnitPrice = worksheet.Cells[row, 3].GetValue<int>(),
                    HaveDate = worksheet.Cells[row, 4].GetValue<bool>(),
                    DateOfManufacture = worksheet.Cells[row, 5].GetValue<DateTime>(),
                    Expiry = worksheet.Cells[row, 6].GetValue<DateTime>(),
                    Trademark = worksheet.Cells[row, 7].GetValue<string>(),
                    Weight = worksheet.Cells[row, 8].GetValue<int>(),
                    LocationId = worksheet.Cells[row, 9].GetValue<int>(),
                    Unit = worksheet.Cells[row, 10].GetValue<string>(),
                    InventoryLevelMin = worksheet.Cells[row, 11].GetValue<int>(),
                    InventoryLevelMax = worksheet.Cells[row, 12].GetValue<int>(),
                    Description = worksheet.Cells[row, 13].GetValue<string>(),
                    SubCategoryId = worksheet.Cells[row, 14].GetValue<int>(),
                    SupplierId = worksheet.Cells[row, 15].GetValue<int>(),
                    Active = worksheet.Cells[row, 16].GetValue<bool>(),
                    Quantity = worksheet.Cells[row, 17].GetValue<int>()
                };

                products.Add(product);
            }

            return products;
        }
    }
}
