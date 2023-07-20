using BusinessObjects.WahoModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Data;
using System.Net.Http.Headers;
using ViewModels.ReturnOrderViewModels;
using Waho.DataService;

namespace WahoClient.Pages.Cashier.ReturnOrders
{
    [Authorize(Roles = "1,2")]

    public class ExportReturnOrderDetailModel : PageModel
    {
        private readonly HttpClient client = null;
        private string returnOrderAPIUrl = "";
        private readonly Author _author;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ExportReturnOrderDetailModel(Author author, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            returnOrderAPIUrl = "https://localhost:7019/waho/ReturnOrders";
            _author = author;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
        }
        private ReturnOrder returnOrder;
        [BindProperty]
        public string successMessage { get; set; }
        public async Task<IActionResult> OnGetAsync(int returnOrderID)
        {
            if (User.Identity?.IsAuthenticated == false)
            {
                return RedirectToPage("/accessDenied", new { message = "do bạn chưa đăng nhập" });
            }
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Request.Cookies["AccessToken"]);
            // get inventoryDetail list
            List<ReturnOrderProduct> inventoryDetailList = new List<ReturnOrderProduct>();
            HttpResponseMessage responseROPs = await client.GetAsync($"{returnOrderAPIUrl}/ROPByReturnID?returnId={returnOrderID}");
            if ((int)responseROPs.StatusCode == 401) return RedirectToPage("/accessDenied");

            string strDataROPs = await responseROPs.Content.ReadAsStringAsync();
            if (responseROPs.IsSuccessStatusCode)
            {
                inventoryDetailList = JsonConvert.DeserializeObject<List<ReturnOrderProduct>>(strDataROPs);
            }
            //var inventoryDetailList = _dataService.GetReturnOrderDetails(returnOrderID);
            HttpResponseMessage responseRO = await client.GetAsync($"{returnOrderAPIUrl}/ROByID?returnId={returnOrderID}");
            if ((int)responseRO.StatusCode == 401) return RedirectToPage("/accessDenied");

            string strDataRO = await responseRO.Content.ReadAsStringAsync();
            if (responseRO.IsSuccessStatusCode)
            {
                returnOrder = JsonConvert.DeserializeObject<ReturnOrder>(strDataRO);
            }
            //returnOrder = _dataService.getReturnOrderByID(returnOrderID);

            // create Excel package
            var package = new ExcelPackage();

            // add a new worksheet to the Excel package
            var worksheet = package.Workbook.Worksheets.Add("ReturnOrderDetail List");

            // set header row
            worksheet.Cells[1, 1].Value = "Mã hoàn đơn :";
            worksheet.Cells[1, 2].Value = returnOrderID;
            worksheet.Cells[1, 4].Value = "Tên khách hàng";
            worksheet.Cells[1, 5].Value = returnOrder.Customer.CustomerName;
            worksheet.Cells[2, 1].Value = "Người tạo đơn";
            worksheet.Cells[2, 2].Value = returnOrder.UserNameNavigation.EmployeeName;
            worksheet.Cells[3, 1].Value = "Ngày tạo";
            worksheet.Cells[3, 2].Value = returnOrder.Date.ToString();
            worksheet.Cells[4, 1].Value = "Số tiền cần trả khách";
            worksheet.Cells[4, 2].Value = returnOrder.PayCustomer + " đồng";
            worksheet.Cells[4, 4].Value = "Số tiền đã trả khách";
            worksheet.Cells[4, 5].Value = returnOrder.PaidCustomer + " đồng";
            worksheet.Cells[5, 1].Value = "Tên sản phẩm";
            worksheet.Cells[5, 2].Value = "Thương hiệu";
            worksheet.Cells[5, 3].Value = "Giá bán";
            worksheet.Cells[5, 4].Value = "Số lượng";

            // set data rows
            int rowIndex = 6;
            foreach (var item in inventoryDetailList)
            {
                worksheet.Cells[rowIndex, 1].Value = item.Product.ProductName;
                worksheet.Cells[rowIndex, 2].Value = item.Product.Trademark;
                worksheet.Cells[rowIndex, 3].Value = item.Product.UnitPrice;
                worksheet.Cells[rowIndex, 4].Value = item.Quantity;
                rowIndex++;
            }


            // save Excel package to a file
            string webRootPath = _hostingEnvironment.WebRootPath;
            string fileName = "ReturnOrderDetail.xlsx";
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
