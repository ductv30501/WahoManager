using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjects.WahoModels;
using System.Net.Http.Headers;
using Waho.DataService;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ViewModels.EmployeeViewModels;
using System.Text;
using ViewModels.SupplierViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace WahoClient.Pages.Admin.Suppliers
{
    [Authorize(Roles = "1")]

    public class CreateModel : PageModel
    {
        private readonly HttpClient client = null;
        private string supplierAPIUrl = "";
        private readonly Author _author;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateModel(Author author, IHttpContextAccessor httpContextAccessor)
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
        public IActionResult OnGet()
        {
            //author
            if (User.Identity?.IsAuthenticated == false)
            {
                return RedirectToPage("/accessDenied", new { message = "do bạn chưa đăng nhập" });
            }
            return Page();
        }

        [BindProperty]
        public SupplierVM Supplier { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Request.Cookies["AccessToken"]);

            var req = HttpContext.Request;
            //get data form form submit 
            string raw_conpanyName = req.Form["companyName"];
            string raw_addres = req.Form["address"];
            string raw_phone = req.Form["phone"];
            string raw_taxCode = req.Form["taxCode"];
            string raw_branch = req.Form["branch"];
            string raw_description = req.Form["description"];
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
            if (string.IsNullOrEmpty(raw_branch))
            {
                //message
                message = "Khi nhánh của cung cấp không được để trống";
                TempData["message"] = message;
                return RedirectToPage("./Index");
            }
            Supplier.Branch = raw_branch;
            Supplier.Description = raw_description;
            Supplier.Active = true;
            // get data from session
            var employeeJson = _httpContextAccessor.HttpContext.Session.GetString("Employee");
            EmployeeVM employeeVM = JsonConvert.DeserializeObject<EmployeeVM>(employeeJson);
            Supplier.WahoId = employeeVM.WahoId;
            // add supplier
            var json = JsonConvert.SerializeObject(Supplier);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(supplierAPIUrl, content);
            if ((int)response.StatusCode == 401) await HttpContext.SignOutAsync("CookieAuthentication");

            if (response.IsSuccessStatusCode)
            {
                successMessage = "Thêm mới nhà cung cấp thành công";
                TempData["successMessage"] = successMessage;
                return RedirectToPage("./Index");
            }
            // success message
            TempData["message"] = "Không thêm được nhà cung cấp";
            return RedirectToPage("./Index");
        }
    }
}
