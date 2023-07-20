using BusinessObjects.WahoModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Data;
using System.Drawing.Printing;
using System.Net.Http.Headers;
using ViewModels.EmployeeViewModels;
using Waho.DataService;

namespace WahoClient.Pages.Admin.Suppliers
{
    [Authorize(Roles = "1")]

    public class ExportSuppliersModel : PageModel
    {
        private readonly HttpClient client = null;
        private string supplierAPIUrl = "";
        private readonly Author _author;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ExportSuppliersModel(Author author, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            supplierAPIUrl = "https://localhost:7019/waho/Suppliers";
            _author = author;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
        }
        [BindProperty]
        public string successMessage { get; set; }
        public async Task<IActionResult> OnGetAsync(string _inventorySheetID)
        {
            //author
            if (User.Identity?.IsAuthenticated == false)
            {
                return RedirectToPage("/accessDenied", new { message = "do bạn chưa đăng nhập" });
            }
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Request.Cookies["AccessToken"]);
            // get data from session
            var employeeJson = _httpContextAccessor.HttpContext.Session.GetString("Employee");
            EmployeeVM employeeVM = JsonConvert.DeserializeObject<EmployeeVM>(employeeJson);
            // get inventoryDetail list
            HttpResponseMessage responsepaging = await client.GetAsync($"{supplierAPIUrl}/getSuppliers?wahoId={employeeVM.WahoId}");
            if ((int)responsepaging.StatusCode == 401) return RedirectToPage("/accessDenied");

            string strDatapaging = await responsepaging.Content.ReadAsStringAsync();
            List<Supplier> suppliers = new List<Supplier>();
            if (responsepaging.IsSuccessStatusCode)
            {
                suppliers = JsonConvert.DeserializeObject<List<Supplier>>(strDatapaging);
            }
            // create Excel package
            var package = new ExcelPackage();

            // add a new worksheet to the Excel package
            var worksheet = package.Workbook.Worksheets.Add("InventoryDetail List");

            // set header row
            worksheet.Cells[1, 1].Value = "Tên công ty";
            worksheet.Cells[1, 2].Value = "Địa chỉ";
            worksheet.Cells[1, 3].Value = "Số điện thoại";
            worksheet.Cells[1, 4].Value = "Mã số thuế";
            worksheet.Cells[1, 5].Value = "Chi nhánh";
            worksheet.Cells[1, 6].Value = "Ghi chú";

            // set data rows
            int rowIndex = 2;
            foreach (var item in suppliers)
            {
                worksheet.Cells[rowIndex, 1].Value = item.CompanyName;
                worksheet.Cells[rowIndex, 2].Value = item.Address;
                worksheet.Cells[rowIndex, 3].Value = item.Phone;
                worksheet.Cells[rowIndex, 4].Value = item.TaxCode;
                worksheet.Cells[rowIndex, 5].Value = item.Branch;
                worksheet.Cells[rowIndex, 6].Value = item.Description;
                rowIndex++;
            }

            // save Excel package to a file
            string webRootPath = _hostingEnvironment.WebRootPath;
            string fileName = "Supplier.xlsx";
            string filePath = Path.Combine(webRootPath, fileName);
            FileInfo file = new FileInfo(filePath);
            package.SaveAs(file);

            // return file download
            var fileProvider = new PhysicalFileProvider(webRootPath);
            var fileInfo = fileProvider.GetFileInfo(fileName);
            var fileStream = fileInfo.CreateReadStream();
            successMessage = "in ra file excel thành công";
            //TempData["successMessage"] = successMessage;
            return File(fileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
