using AutoMapper;
using BusinessObjects.WahoModels;
using DataAccess.AutoMapperConfig;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Data;
using System.Net.Http.Headers;
using Waho.DataService;

namespace WahoClient.Pages.WarehouseStaff.InventorySheetManager
{
    [Authorize(Roles = "1,3")]

    public class ExportInventoryModel : PageModel
    {
        private readonly HttpClient client = null;
        private string inventoryAPIUrl = "";
        private readonly Author _author;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static readonly IMapper _mapperDetail = InventorySheetDetailMapper.ConfigureMToVM();
        private static readonly IMapper _mapper = ProductConfigMapper.ConfigureMToVM();
        private readonly IWebHostEnvironment _hostingEnvironment;
        private InventorySheet inventorySheet;
        [BindProperty]
        public string successMessage { get; set; }
        public ExportInventoryModel(Author author, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            inventoryAPIUrl = "https://localhost:7019/waho/InventorySheets";
            _author = author;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<IActionResult> OnGetAsync(string _inventorySheetID)
        {
            if (User.Identity?.IsAuthenticated == false)
            {
                return RedirectToPage("/accessDenied", new { message = "do bạn chưa đăng nhập" });
            }
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Request.Cookies["AccessToken"]);

            // get inventoryDetail list
            Int32 inventorySheetID = Int32.Parse(HttpContext.Request.Query["inventorySheetID"]);
            List<InventorySheetDetail> inventoryDetailList = new List<InventorySheetDetail>();
            // get inventory sheet by id
            HttpResponseMessage responseInventory = await client.GetAsync($"{inventoryAPIUrl}/getInventorySheetById?inventorySheetId={inventorySheetID}");
            if ((int)responseInventory.StatusCode == 401) return RedirectToPage("/accessDenied");

            string strDataInventory = await responseInventory.Content.ReadAsStringAsync();
            if (responseInventory.IsSuccessStatusCode)
            {
                inventorySheet = JsonConvert.DeserializeObject<InventorySheet>(strDataInventory);
            }
            // get all inventory details of inventory
            HttpResponseMessage responseInventoryDetailAll = await client.GetAsync($"{inventoryAPIUrl}/getInventoryDetails?inventorySheetId={inventorySheetID}");
            if ((int)responseInventoryDetailAll.StatusCode == 401) return RedirectToPage("/accessDenied");

            string strDataInventoryDetailAl = await responseInventoryDetailAll.Content.ReadAsStringAsync();
            if (responseInventoryDetailAll.IsSuccessStatusCode)
            {
                inventoryDetailList = JsonConvert.DeserializeObject<List<InventorySheetDetail>>(strDataInventoryDetailAl);
            }
            // create Excel package
            var package = new ExcelPackage();

            // add a new worksheet to the Excel package
            var worksheet = package.Workbook.Worksheets.Add("InventoryDetail List");

            Int64 totalDifference = 0;
            // set header row
            worksheet.Cells[1, 1].Value = "Mã phiếu kiểm kho :";
            worksheet.Cells[1, 2].Value = inventorySheetID;
            worksheet.Cells[2, 1].Value = "Người kiểm phiếu";
            worksheet.Cells[2, 2].Value = inventorySheet.UserNameNavigation.EmployeeName;
            worksheet.Cells[3, 1].Value = "Ngày tạo";
            worksheet.Cells[3, 2].Value = inventorySheet.Date.ToString();
            worksheet.Cells[5, 1].Value = "Tên sản phẩm";
            worksheet.Cells[5, 2].Value = "Số lượng thực tế trong kho";
            worksheet.Cells[5, 3].Value = "Số lượng trên web";
            worksheet.Cells[5, 4].Value = "Chênh lệch thực tế";
            worksheet.Cells[5, 5].Value = "Số tiền chênh";

            // set data rows
            int rowIndex = 6;
            foreach (var item in inventoryDetailList)
            {
                var difference = item.CurNwareHouse - item.Product.Quantity;
                var totalDiffEach = difference * item.Product.UnitPrice;
                totalDifference += totalDiffEach;
                worksheet.Cells[rowIndex, 1].Value = item.Product.ProductName;
                worksheet.Cells[rowIndex, 2].Value = item.CurNwareHouse;
                worksheet.Cells[rowIndex, 3].Value = item.Product.Quantity;
                worksheet.Cells[rowIndex, 4].Value = difference;
                worksheet.Cells[rowIndex, 5].Value = totalDiffEach;
                rowIndex++;
            }
            worksheet.Cells[4, 1].Value = "Tổng tiền chênh";
            worksheet.Cells[4, 2].Value = totalDifference + " đồng";

            // save Excel package to a file
            string webRootPath = _hostingEnvironment.WebRootPath;
            string fileName = "InventoryDetail.xlsx";
            string filePath = Path.Combine(webRootPath, fileName);
            FileInfo file = new FileInfo(filePath);
            package.SaveAs(file);

            // return file download
            var fileProvider = new PhysicalFileProvider(webRootPath);
            var fileInfo = fileProvider.GetFileInfo(fileName);
            var fileStream = fileInfo.CreateReadStream();
            //successMessage = "in ra file excel thành công";
            //TempData["successMessage"] = successMessage;
            return File(fileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
