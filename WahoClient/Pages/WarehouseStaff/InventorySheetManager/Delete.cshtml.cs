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
using ViewModels.InventorySheetViewModels;
using Newtonsoft.Json;
using AutoMapper;
using DataAccess.AutoMapperConfig;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Authentication;

namespace WahoClient.Pages.WarehouseStaff.InventorySheetManager
{
    [Authorize(Roles = "1,3")]

    public class DeleteModel : PageModel
    {
        private readonly HttpClient client = null;
        private string inventoryAPIUrl = "";
        private readonly Author _author;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static readonly IMapper _mapper = InventorySheetMapper.ConfigureMToVM();
        public DeleteModel(Author author, IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            inventoryAPIUrl = "https://localhost:7019/waho/InventorySheets";
            _author = author;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public InventorySheet InventorySheet { get; set; } = default!;
        public string message { get; set; }
        public string successMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int inventorySheetID)
        {
            if (User.Identity?.IsAuthenticated == false)
            {
                return RedirectToPage("/accessDenied", new { message = "do bạn chưa đăng nhập" });
            }
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Request.Cookies["AccessToken"]);

            InventorySheetVM sheetVM = new InventorySheetVM();
            // get inventory sheet by id
            HttpResponseMessage responseInventory = await client.GetAsync($"{inventoryAPIUrl}/getInventorySheetById?inventorySheetId={inventorySheetID}");
            if ((int)responseInventory.StatusCode == 401) await HttpContext.SignOutAsync("CookieAuthentication");

            string strDataInventory = await responseInventory.Content.ReadAsStringAsync();
            if (responseInventory.IsSuccessStatusCode)
            {
                InventorySheet = JsonConvert.DeserializeObject<InventorySheet>(strDataInventory);
                sheetVM = _mapper.Map<InventorySheetVM>(InventorySheet);
            }
            if (sheetVM != null)
            {
                sheetVM.Active = false;
                // update inventory sheet
                var json = JsonConvert.SerializeObject(sheetVM);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(inventoryAPIUrl, content);
                if ((int)response.StatusCode == 401) await HttpContext.SignOutAsync("CookieAuthentication");

                // message
                if (response.IsSuccessStatusCode)
                {
                    //success message
                    successMessage = "Xóa thành công phiếu kiểm kho ra khỏi danh sách";
                    TempData["successMessage"] = successMessage;
                    return RedirectToPage("./Index");
                }
                message = "Xóa thất bại";
                TempData["message"] = message;
                return RedirectToPage("./Index");
            }
            message = "không tìm thấy phiếu kiểm kho";
            TempData["message"] = message;
            return RedirectToPage("./Index");
            
        }

    }
}
