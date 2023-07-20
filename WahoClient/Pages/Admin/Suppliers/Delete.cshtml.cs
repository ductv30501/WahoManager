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
using System.Drawing.Printing;
using ViewModels.EmployeeViewModels;
using AutoMapper;
using DataAccess.AutoMapperConfig;
using ViewModels.SupplierViewModels;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Authentication;

namespace WahoClient.Pages.Admin.Suppliers
{
    [Authorize(Roles = "1")]

    public class DeleteModel : PageModel
    {
        private readonly HttpClient client = null;
        private string supplierAPIUrl = "";
        private readonly Author _author;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static readonly IMapper _mapper = SupplierMapper.ConfigureMToVM();
        public DeleteModel(Author author, IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            supplierAPIUrl = "https://localhost:7019/waho/Suppliers";
            _author = author;
            _httpContextAccessor = httpContextAccessor;
        }
        public string message { get; set; }
        public string successMessage { get; set; }
        [BindProperty]
        public Supplier Supplier { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int supplierID)
        {
            //author
            if (User.Identity?.IsAuthenticated == false)
            {
                return RedirectToPage("/accessDenied", new { message = "do bạn chưa đăng nhập" });
            }
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Request.Cookies["AccessToken"]);

            HttpResponseMessage responsepaging = await client.GetAsync($"{supplierAPIUrl}/getByID?supId={supplierID}");
            if ((int)responsepaging.StatusCode == 401) return RedirectToPage("/accessDenied");

            string strDatapaging = await responsepaging.Content.ReadAsStringAsync();

            if (responsepaging.IsSuccessStatusCode)
            {
                Supplier = JsonConvert.DeserializeObject<Supplier>(strDatapaging);
            }
            if (Supplier != null)
            {
                Supplier.Active = false;
                SupplierVM supplierVM = _mapper.Map<SupplierVM>(Supplier);
                var json = JsonConvert.SerializeObject(supplierVM);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(supplierAPIUrl, content);
                if ((int)response.StatusCode == 401) return RedirectToPage("/accessDenied");

                if (response.IsSuccessStatusCode)
                {
                    // message
                    successMessage = "Xóa thành công nhà cung cấp ra khỏi danh sách";
                    TempData["successMessage"] = successMessage;
                    return RedirectToPage("./Index");
                }
            }
            message = "không tìm thấy nhà cung cấp";
            TempData["message"] = message;
            return RedirectToPage("./Index");
        }

    }
}
