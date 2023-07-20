using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.WahoModels;
using System.Net.Http.Headers;
using Waho.DataService;
using ViewModels.SupplierViewModels;
using Newtonsoft.Json;
using ViewModels.EmployeeViewModels;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Authentication;

namespace WahoClient.Pages.Admin.Suppliers
{
    [Authorize(Roles = "1")]

    public class EditModel : PageModel
    {
        private readonly HttpClient client = null;
        private string supplierAPIUrl = "";
        private readonly Author _author;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EditModel(Author author, IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            supplierAPIUrl = "https://localhost:7019/waho/Suppliers";
            _author = author;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public SupplierVM Supplier { get; set; } = default!;
        public string message { get; set; }
        public string successMessage { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            //author
            if (User.Identity?.IsAuthenticated == false)
            {
                return RedirectToPage("/accessDenied", new { message = "do bạn chưa đăng nhập" });
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Request.Cookies["AccessToken"]);

            var req = HttpContext.Request;
            //get data form form submit 
            string raw_supplierID = req.Form["supplierIDUpdate"];
            string raw_conpanyName = req.Form["companyNameUpdate"];
            string raw_addres = req.Form["addressUpdate"];
            string raw_phone = req.Form["phoneUpdate"];
            string raw_taxCode = req.Form["taxCodeUpdate"];
            string raw_branch = req.Form["branchUpdate"];
            string raw_description = req.Form["descriptionUpdate"];

            Supplier.SupplierId = Int32.Parse(raw_supplierID);
            //validate
            if (string.IsNullOrEmpty(raw_conpanyName))
            {
                //message
                message = "tên nhà cung cấp không được để trống";
                TempData["message"] = message;
                return RedirectToPage("./Index");
            }
            Supplier.CompanyName = raw_conpanyName;
            if (string.IsNullOrEmpty(raw_addres))
            {
                //message
                message = "địa chỉ cung cấp không được để trống";
                TempData["message"] = message;
                return RedirectToPage("./Index");
            }
            Supplier.Address = raw_addres;
            if (string.IsNullOrEmpty(raw_phone))
            {
                //message
                message = "Số điện thoại của cung cấp không được để trống";
                TempData["message"] = message;
                return RedirectToPage("./Index");
            }
            Supplier.Phone = raw_phone;
            if (string.IsNullOrEmpty(raw_taxCode))
            {
                //message
                message = "Mã số thuế của cung cấp không được để trống";
                TempData["message"] = message;
                return RedirectToPage("./Index");
            }
            Supplier.TaxCode = raw_taxCode;
            Supplier.Branch = raw_branch;
            Supplier.Description = raw_description;
            Supplier.Active = true;
            // get data from session
            var employeeJson = _httpContextAccessor.HttpContext.Session.GetString("Employee");
            EmployeeVM employeeVM = JsonConvert.DeserializeObject<EmployeeVM>(employeeJson);
            Supplier.WahoId = employeeVM.WahoId;
            // update
            var json = JsonConvert.SerializeObject(Supplier);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(supplierAPIUrl, content);
            if ((int)response.StatusCode == 401) await HttpContext.SignOutAsync("CookieAuthentication");

            if (response.IsSuccessStatusCode)
            {
                successMessage = "Chỉnh sửa thông tin nhà cung cấp thành công";
                TempData["successMessage"] = successMessage;
                return RedirectToPage("./Index");
            }
            // success message
            TempData["successMessage"] = "Chỉnh sửa thông tin nhà cung cấp thất bại";
            return RedirectToPage("./Index");
        }

       
    }
}
