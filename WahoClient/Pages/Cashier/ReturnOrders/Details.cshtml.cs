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
using ViewModels.ReturnOrderViewModels;
using ViewModels.EmployeeViewModels;
using System.Text;
using ViewModels.OrderViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Authentication;

namespace WahoClient.Pages.Cashier.ReturnOrders
{
    [Authorize(Roles = "1,2")]

    public class DetailsModel : PageModel
    {
        private readonly HttpClient client = null;
        private string returnOrderAPIUrl = "";
        private readonly Author _author;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DetailsModel(Author author, IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            returnOrderAPIUrl = "https://localhost:7019/waho/ReturnOrders";
            _author = author;
            _httpContextAccessor = httpContextAccessor;
        }
        //message
        public string message { get; set; }
        public string successMessage { get; set; }
        // paging
        [BindProperty(SupportsGet = true)]
        public int pageSize { get; set; } = 10;

        [BindProperty(SupportsGet = true)]
        public int pageIndex { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int TotalCount { get; set; } = 0;

        private string raw_pageSize;
        [BindProperty(SupportsGet = true)]
        public int _returnOrderID { get; set; }

        public ReturnOrder ReturnOrder { get; set; } = default!;
        public List<ReturnOrderProduct> returnOrderProducts { get; set; }

        public async Task<IActionResult> OnGetAsync(int returnOrderID)
        {
            if (User.Identity?.IsAuthenticated == false)
            {
                return RedirectToPage("/accessDenied", new { message = "do bạn chưa đăng nhập" });
            }
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Request.Cookies["AccessToken"]);
            // get data from form
            raw_pageSize = HttpContext.Request.Query["pageSize"];
            if (returnOrderID != 0)
            {
                _returnOrderID = returnOrderID;
            }
            if (HttpContext.Request.HasFormContentType == true)
            {

                if (!string.IsNullOrEmpty(HttpContext.Request.Form["returnOrderID"]))
                {
                    _returnOrderID = Int32.Parse(HttpContext.Request.Form["returnOrderID"]);
                }
            }

            if (!string.IsNullOrEmpty(raw_pageSize))
            {
                pageSize = int.Parse(raw_pageSize);
            }
            // get return order
            if (returnOrderID == null)
            {
                //message
                message = "Không tìm thấy mã hoàn đơn";
                TempData["message"] = message;
                return RedirectToPage("./Index");
            }
            // get return order 
            ReturnOrder returnorder = new ReturnOrder();
            HttpResponseMessage responseRO = await client.GetAsync($"{returnOrderAPIUrl}/ROByID?returnId={_returnOrderID}");
            if ((int)responseRO.StatusCode == 401) await HttpContext.SignOutAsync("CookieAuthentication");

            string strDataRO = await responseRO.Content.ReadAsStringAsync();
            if (responseRO.IsSuccessStatusCode)
            {
                returnorder = JsonConvert.DeserializeObject<ReturnOrder>(strDataRO);
            }
            if (returnorder == null)
            {
                //message
                message = "Không tìm thấy hoàn đơn tương ứng returnorder";
                TempData["message"] = message;
                return RedirectToPage("./Index");
            }
            else
            {
                ReturnOrder = returnorder;
            }
            // get list product of return order and paging
            // count return order product
            List<ReturnOrderProduct> _rops = new List<ReturnOrderProduct>();
            HttpResponseMessage responseROPs = await client.GetAsync($"{returnOrderAPIUrl}/ROPByReturnID?returnId={_returnOrderID}");
            if ((int)responseROPs.StatusCode == 401) await HttpContext.SignOutAsync("CookieAuthentication");

            string strDataROPs = await responseROPs.Content.ReadAsStringAsync();
            if (responseROPs.IsSuccessStatusCode)
            {
                _rops = JsonConvert.DeserializeObject<List<ReturnOrderProduct>>(strDataROPs);
                TotalCount = _rops.Count;
            }

            // get return order product paging
            if (_rops.Count != 0)
            {
                HttpResponseMessage responseROPsPaging = await client.GetAsync($"{returnOrderAPIUrl}/ROPSPaging?pageIndex={pageIndex}&pageSize={pageSize}&id={_returnOrderID}");
                if ((int)responseROPsPaging.StatusCode == 401) await HttpContext.SignOutAsync("CookieAuthentication");

                string strDataROPsPaging = await responseROPsPaging.Content.ReadAsStringAsync();
                if (responseROPsPaging.IsSuccessStatusCode)
                {
                    returnOrderProducts = JsonConvert.DeserializeObject<List<ReturnOrderProduct>>(strDataROPsPaging);
                }
                return Page();
            }
            //message
            message = "Không tìm thấy hoàn đơn tương ứng";
            //TempData["message"] = message;
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Request.Cookies["AccessToken"]);
            // get data from session
            string employeeJson = _httpContextAccessor.HttpContext.Session.GetString("Employee");
            EmployeeVM eSession = JsonConvert.DeserializeObject<EmployeeVM>(employeeJson);
            ReturnOrderVM _returnOrderUpdate = new ReturnOrderVM();
            //get data form form submit 
            var req = HttpContext.Request;
            string raw_EmployeeID = req.Form["employeeID"];
            string raw_returnOrderID = req.Form["returnOrderID"];
            string raw_customerID = req.Form["customerID"];
            string raw_date = req.Form["date"];
            string raw_state = req.Form["state"];
            string raw_description = req.Form["description"];
            string raw_payCustomer = req.Form["payCustomer"];
            string raw_paidCustomer = req.Form["paidCustomer"];
            _returnOrderUpdate.ReturnOrderId = Int32.Parse(raw_returnOrderID);
            _returnOrderUpdate.UserName = raw_EmployeeID;
            _returnOrderUpdate.CustomerId = Int32.Parse(raw_customerID);
            if (string.IsNullOrWhiteSpace(raw_date))
            {
                //message
                message = "Ngày tạo đơn không được để trống";
                TempData["ErrorMessage"] = message;
                return Page();
            }
            _returnOrderUpdate.Date = DateTime.Parse(raw_date);
            _returnOrderUpdate.Description = raw_description;
            _returnOrderUpdate.Active = true;
            if (string.IsNullOrWhiteSpace(raw_paidCustomer))
            {
                //message
                message = "Số tiền đã trả khách không được để trống";
                TempData["ErrorMessage"] = message;
                return Page();
            }
            _returnOrderUpdate.PaidCustomer = decimal.Parse(raw_paidCustomer);
            if (string.IsNullOrWhiteSpace(raw_payCustomer))
            {
                //message
                message = "Số tiền cần trả khách không được để trống";
                TempData["ErrorMessage"] = message;
                return Page();
            }
            if (string.IsNullOrWhiteSpace(raw_state))
            {
                _returnOrderUpdate.State = false;
            }
            else
            {
                _returnOrderUpdate.State = true;
            }

            _returnOrderUpdate.PayCustomer = decimal.Parse(raw_payCustomer);
            _returnOrderUpdate.WahoId = eSession.WahoId;
            // update return order
            var json = JsonConvert.SerializeObject(_returnOrderUpdate);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(returnOrderAPIUrl, content);
            if ((int)response.StatusCode == 401) await HttpContext.SignOutAsync("CookieAuthentication");

            if (response.IsSuccessStatusCode)
            {
                successMessage = "Chỉnh sửa thông tin phiếu thành công";
                TempData["SuccessMessage"] = successMessage;
                return RedirectToPage("./Index");
            }
            //_context.Attach(_returnOrderUpdate).State = EntityState.Modified;
            //    await _context.SaveChangesAsync();
            //success message
            message = "Chỉnh sửa thông tin phiếu thất bại";
                TempData["ErrorMessage"] = message;
                return RedirectToPage("./Index");

        }
    }
}
