using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjects.WahoModels;
using AutoMapper;
using DataAccess.AutoMapperConfig;
using System.Net.Http.Headers;
using Waho.DataService;
using Microsoft.Extensions.FileProviders;
using OfficeOpenXml;
using Microsoft.EntityFrameworkCore;
using ViewModels.InventorySheetViewModels;
using Newtonsoft.Json;
using System.Text;
using ViewModels.EmployeeViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Authentication;
using System.Net;

namespace WahoClient.Pages.WarehouseStaff.InventorySheetManager
{
    [Authorize(Roles = "1,3")]

    public class CreateModel : PageModel
    {
        private readonly HttpClient client = null;
        private string inventoryAPIUrl = "";
        private readonly Author _author;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileProvider _fileProvider;
        private static readonly IMapper _mapperDetail = InventorySheetDetailMapper.ConfigureMToVM();
        private static readonly IMapper _mapper = ProductConfigMapper.ConfigureMToVM();
        public CreateModel(Author author, IHttpContextAccessor httpContextAccessor, IFileProvider fileProvider)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            inventoryAPIUrl = "https://localhost:7019/waho/InventorySheets";
            _author = author;
            _httpContextAccessor = httpContextAccessor;
            _fileProvider = fileProvider;
        }
        public string message { get; set; }
        public string successMessage { get; set; }
        [BindProperty]
        public InventorySheetVM InventorySheet { get; set; }
        List<InventorySheetDetail> inventorySheetDetails { get; set; }
        [BindProperty]
        public string ExcelFile { get; set; }
        private int inventoryID;

        public IActionResult OnGet()
        {
            if (User.Identity?.IsAuthenticated == false)
            {
                return RedirectToPage("/accessDenied", new { message = "do bạn chưa đăng nhập" });
            }

            // Lấy thông tin file từ IFileProvider
            IFileInfo fileInfo = _fileProvider.GetFileInfo("Inventory.xlsx");
            if (fileInfo.Exists)
            {
                // Đọc nội dung file và trả về file download
                var stream = new MemoryStream();
                using (var fileStream = fileInfo.CreateReadStream())
                {
                    fileStream.CopyTo(stream);
                }
                stream.Position = 0;
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "templateUploadInventory.xlsx");
            }
            else
            {
                // messagse
                message = "file không tồn tại";
                TempData["message"] = message;
                return RedirectToPage("./Index");
            }
        }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Request.Cookies["AccessToken"]);

            // get data from session
            string employeeJson = _httpContextAccessor.HttpContext.Session.GetString("Employee");
            EmployeeVM eSession = JsonConvert.DeserializeObject<EmployeeVM>(employeeJson);
            var req = HttpContext.Request;
            //get data form form submit 
            string raw_employeeID = req.Form["employeeID"];
            string raw_date = req.Form["date"];
            string raw_description = req.Form["description"];
            if (!string.IsNullOrEmpty(raw_employeeID))
            {
                InventorySheet.UserName = raw_employeeID;
            }
            if (string.IsNullOrWhiteSpace(raw_date))
            {
                // messagse
                message = "ngày kiểm kho không được để trống";
                TempData["message"] = message;
                return RedirectToPage("./Index");
            }
            InventorySheet.Date = DateTime.Parse(raw_date);
            InventorySheet.Active = true;
            InventorySheet.WahoId = eSession.WahoId;
            if (!string.IsNullOrEmpty(raw_description))
            {
                InventorySheet.Description = raw_description;
            }

            //add file product 
            if (string.IsNullOrEmpty(req.Form["ExcelFile"]))
            {
                // messagse
                message = "đường dẫn file trống, bạn cần chọn file trước khi gửi form";
                TempData["message"] = message;
                return RedirectToPage("./Index");
            }
            else
            {
                // add inventory sheet return inventory sheet id
                var json = JsonConvert.SerializeObject(InventorySheet);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(inventoryAPIUrl, content);
                if ((int)response.StatusCode == 401) await HttpContext.SignOutAsync("CookieAuthentication");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var PostInventorySheet = JsonConvert.DeserializeObject<PostInventorySheetVM>(responseContent);
                    inventoryID = PostInventorySheet.InventorySheetId;
                }
                //_context.InventorySheets.Add(InventorySheet);
                //await _context.SaveChangesAsync();
                IFileInfo fileInfo = _fileProvider.GetFileInfo(req.Form["ExcelFile"]);
                ExcelFile = fileInfo.PhysicalPath;
            }
            //add list product
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
                    List<InventorySheetDetailVM> inventorySheetDetails = ReadExcel(ExcelFile);
                    if (inventorySheetDetails == null)
                    {
                        // messagse
                        message = "trong file không có sản phẩm nào";
                        TempData["message"] = message;
                        return RedirectToPage("./Index");
                    }
                    else
                    {

                        // add list inventory sheet details
                        var jsonDetail = JsonConvert.SerializeObject(inventorySheetDetails);
                        var contentDetail = new StringContent(jsonDetail, Encoding.UTF8, "application/json");
                        var responseDetail = await client.PostAsync($"{inventoryAPIUrl}/detail", contentDetail);
                        if ((int)responseDetail.StatusCode == 401) await HttpContext.SignOutAsync("CookieAuthentication");

                        //await _context.BulkInsertAsync(inventorySheetDetails);
                        successMessage = $"{inventorySheetDetails.Count} sản phẩm được thêm vào phiếu kiểm thành công";
                        TempData["successMessage"] = successMessage;
                        return RedirectToPage("./Index");
                    }
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    TempData["message"] = "Bạn đã tải lên không đúng file, vui lòng nhập đúng theo mẫu";
                    return RedirectToPage("./Index");
                }
            }

        }
        private List<InventorySheetDetailVM> ReadExcel(string path)
        {
            using var package = new ExcelPackage(new FileInfo(path));
            var worksheet = package.Workbook.Worksheets[0];
            var inventorySheetDetails = new List<InventorySheetDetailVM>();
            for (int row = 2; row <= worksheet.Dimension.Rows; row++)
            {
                var inventoryDetail = new InventorySheetDetailVM
                {
                    InventorySheetId = inventoryID,
                    ProductId = worksheet.Cells[row, 1].GetValue<int>(),
                    CurNwareHouse = worksheet.Cells[row, 18].GetValue<int>()
                };

                inventorySheetDetails.Add(inventoryDetail);
            }

            return inventorySheetDetails;
        }
    }
}
