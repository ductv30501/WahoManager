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
using ViewModels.OrderViewModels;
using ViewModels.ReturnOrderViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Authentication;

namespace WahoClient.Pages.Cashier.ReturnOrders
{
    [Authorize(Roles = "1,2")]

    public class CreateModel : PageModel
    {
        private readonly HttpClient client = null;
        private string returnOrderAPIUrl = "";
        private string billAPIUrl = "";
        private string oderAPIUrl = "";
        private readonly Author _author;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateModel(Author author, IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            returnOrderAPIUrl = "https://localhost:7019/waho/ReturnOrders";
            billAPIUrl = "https://localhost:7019/waho/Bills";
            oderAPIUrl = "https://localhost:7019/waho/Orders";
            _author = author;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult OnGet()
        {
            if (User.Identity?.IsAuthenticated == false)
            {
                return RedirectToPage("/accessDenied", new { message = "do bạn chưa đăng nhập" });
            }
            return Page();
        }

        [BindProperty(SupportsGet = true)]
        public List<Product> products { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<Customer> customers { get; set; }
        private Employee employee { get; set; }
        [BindProperty(SupportsGet = true)]
        public int billCategory { get; set; }

        [BindProperty]
        public ReturnOrderVM _ReturnOrder { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Request.Cookies["AccessToken"]);

            string customerId = HttpContext.Request.Form["customerId"];
            string total = HttpContext.Request.Form["total"];
            string idBill = HttpContext.Request.Form["idBill"];
            string state = HttpContext.Request.Form["state"];
            string paidCustomer = HttpContext.Request.Form["paidCustomer"];
            string description = HttpContext.Request.Form["description"];
            string listBillDetail = HttpContext.Request.Form["listBillDetail"];
            List<ReturnOrderProductVM> billDetails = JsonConvert.DeserializeObject<List<ReturnOrderProductVM>>(listBillDetail);
            int bill_id = Int32.Parse(idBill);
            // get data from session
            string employeeJson = _httpContextAccessor.HttpContext.Session.GetString("Employee");
            EmployeeVM eSession = JsonConvert.DeserializeObject<EmployeeVM>(employeeJson);
            //validate number of  product
            if (billCategory == 1)
            {
                //get details in the bill of customer
                List<BillDetail> _billDetails = new List<BillDetail>();

                HttpResponseMessage responseBDetail = await client.GetAsync($"{billAPIUrl}/detailById?billId={bill_id}");
                if ((int)responseBDetail.StatusCode == 401) await HttpContext.SignOutAsync("CookieAuthentication");

                string strDataBDetail = await responseBDetail.Content.ReadAsStringAsync();
                if (responseBDetail.IsSuccessStatusCode)
                {
                    _billDetails = JsonConvert.DeserializeObject<List<BillDetail>>(strDataBDetail);
                }
                //List<BillDetail> _billDetails = _context.BillDetails.Where(b => b.BillId == Int32.Parse(idBill)).ToList();
                Boolean check = false;
                foreach (var item in _billDetails)
                {
                    // the details return
                    foreach (var itemReturn in billDetails)
                    {
                        if (item.ProductId == itemReturn.ProductId)
                        {
                            if (itemReturn.Quantity > item.Quantity)
                            {
                                //message error
                                TempData["ErrorMessage"] = "Số lượng trả hàng không được vượt quá số lượng đã mua!";
                                return Page();
                            }
                            if (item.Discount != itemReturn.Discount)
                            {
                                //message error
                                TempData["ErrorMessage"] = "Khác giá trị giảm giá!";
                                return Page();
                            }
                            check = true;
                        }
                    }
                }
                if (!check)
                {
                    //message error
                    TempData["ErrorMessage"] = "Không tìm thấy sản phẩm trong hóa đơn!";
                    return Page();
                }
            }
            else
            {
                // get list order detail by order id (bill id) 
                List<OderDetail> _OderDetail = new List<OderDetail>();
                HttpResponseMessage responseODetail = await client.GetAsync($"{oderAPIUrl}/orderDetailsById?orderId={bill_id}");
                if ((int)responseODetail.StatusCode == 401) await HttpContext.SignOutAsync("CookieAuthentication");

                string strDataODetail = await responseODetail.Content.ReadAsStringAsync();
                if (responseODetail.IsSuccessStatusCode)
                {
                    _OderDetail = JsonConvert.DeserializeObject<List<OderDetail>>(strDataODetail);
                }
                //List<OderDetail> _OderDetail = _context.OderDetails.Where(b => b.OderId == Int32.Parse(idBill)).ToList();
                Boolean check = false;
                foreach (var item in _OderDetail)
                {
                    foreach (var itemReturn in billDetails)
                    {
                        if (item.ProductId == itemReturn.ProductId)
                        {
                            if (itemReturn.Quantity > item.Quantity)
                            {
                                //message error
                                TempData["ErrorMessage"] = "Số lượng trả hàng không được vượt quá số lượng đã mua!";
                                return Page();
                            }
                            if (item.Discount != itemReturn.Discount)
                            {
                                //message error
                                TempData["ErrorMessage"] = "Khác giá trị giảm giá!";
                                return Page();
                            }
                            check = true;
                        }
                    }
                }
                if (!check)
                {
                    //message error
                    TempData["ErrorMessage"] = "Không tìm thấy sản phẩm trong đơn vận!";
                    return Page();
                }
            }
            // validate product and bill id already in return order
            // return order have billID = idBill 
            List<ReturnOrder> _returnorderCheck = new List<ReturnOrder>();
            HttpResponseMessage responseRO = await client.GetAsync($"{returnOrderAPIUrl}/returnOrdersByBillId?billId={bill_id}&wahoId={eSession.WahoId}");
            if ((int)responseRO.StatusCode == 401) await HttpContext.SignOutAsync("CookieAuthentication");

            string strDataRO = await responseRO.Content.ReadAsStringAsync();
            if (responseRO.IsSuccessStatusCode)
            {
                _returnorderCheck = JsonConvert.DeserializeObject<List<ReturnOrder>>(strDataRO);
            }
            //ReturnOrder _returnorderCheck = _context.ReturnOrders.FirstOrDefault(r => r.BillId == Int32.Parse(idBill));
            if (_returnorderCheck.Count != 0)
            {
                // list return product of the bill have billID = idbill
                List<ReturnOrderProduct> returnOrderProducts = new List<ReturnOrderProduct>();
                foreach (var rtc in _returnorderCheck)
                {
                    List<ReturnOrderProduct> _rops = new List<ReturnOrderProduct>();
                    HttpResponseMessage responseROPs = await client.GetAsync($"{returnOrderAPIUrl}/ROPByReturnID?returnId={rtc.ReturnOrderId}");
                    if ((int)responseROPs.StatusCode == 401) await HttpContext.SignOutAsync("CookieAuthentication");

                    string strDataROPs = await responseROPs.Content.ReadAsStringAsync();
                    if (responseROPs.IsSuccessStatusCode)
                    {
                        _rops = JsonConvert.DeserializeObject<List<ReturnOrderProduct>>(strDataROPs);
                        returnOrderProducts.AddRange(_rops);
                    }
                }
                //List<ReturnOrderProduct> returnOrderProducts = _context.ReturnOrderProducts
                //                                                .Include(r => r.Product)
                //                                                .Where(r => r.ReturnOrderId == _returnorderCheck.ReturnOrderId)
                //                                                .ToList();
                //  billdetail : list product return 
                if (returnOrderProducts.Count != 0)
                {
                    foreach (var billDetail in billDetails)
                    {
                        int totalReturned = 0;
                        ReturnOrderProduct rOP = new ReturnOrderProduct();
                        foreach (var r in returnOrderProducts)
                        {
                            if (billDetail.ProductId == r.ProductId)
                            {
                                totalReturned += r.Quantity;
                                rOP = r;
                                rOP.Product = r.Product;
                            }
                        }
                        if (totalReturned > 0)
                        {
                            if (billCategory == 1)
                            {
                                // detail of the product of the bill bought
                                BillDetail detailBill = new BillDetail();
                                HttpResponseMessage responseBDs = await client.GetAsync($"{billAPIUrl}/detailByIdAndProId?billId={bill_id}&productId={rOP.ProductId}");
                                if ((int)responseBDs.StatusCode == 401) await HttpContext.SignOutAsync("CookieAuthentication");

                                string strDataBDs = await responseBDs.Content.ReadAsStringAsync();
                                if (responseBDs.IsSuccessStatusCode)
                                {
                                    detailBill = JsonConvert.DeserializeObject<BillDetail>(strDataBDs);
                                }
                                //var detailBill = _context.BillDetails.Where(r => r.ProductId == r.ProductId)
                                //                                        .Where(r => r.BillId == Int32.Parse(idBill))
                                //                                        .FirstOrDefault();
                                if ((detailBill.Quantity - totalReturned) <= 0)
                                {
                                    //message error
                                    TempData["ErrorMessage"] = "Sản phẩm " + rOP.Product.ProductName + " đã được hoàn hết số lượng trong bill!" +
                                                                "Số lượng có thể trả" + (detailBill.Quantity - totalReturned);
                                    return Page();
                                }
                            }
                            else
                            {
                                // detail of the product of the ordered
                                OderDetail detailBill = new OderDetail();
                                HttpResponseMessage responseODs = await client.GetAsync($"{oderAPIUrl}/OrderDetailByIDProID?orderId={bill_id}&productId={rOP.ProductId}");
                                if ((int)responseODs.StatusCode == 401) await HttpContext.SignOutAsync("CookieAuthentication");

                                string strDataODs = await responseODs.Content.ReadAsStringAsync();
                                if (responseODs.IsSuccessStatusCode)
                                {
                                    detailBill = JsonConvert.DeserializeObject<OderDetail>(strDataODs);
                                }
                                //var detailBill = _context.OderDetails.Where(r => r.ProductId == r.ProductId)
                                //                                        .Where(r => r.OderId == Int32.Parse(idBill))
                                //                                        .FirstOrDefault();
                                if ((detailBill.Quantity - totalReturned) <= 0)
                                {
                                    //message error
                                    TempData["ErrorMessage"] = "Sản phẩm " + rOP.Product.ProductName + " đã được hoàn hết số lượng trong order!" +
                                                                "Số lượng có thể trả" + (detailBill.Quantity - totalReturned);
                                    return Page();
                                }
                            }
                        }
                    }
                }
            }
            //add information for return order
            _ReturnOrder.UserName = eSession.UserName;
            _ReturnOrder.CustomerId = int.Parse(customerId);
            _ReturnOrder.Date = DateTime.Now;
            _ReturnOrder.Active = true;
            _ReturnOrder.PayCustomer = decimal.Parse(total);
            _ReturnOrder.PaidCustomer = decimal.Parse(paidCustomer);
            _ReturnOrder.Description = description;
            _ReturnOrder.BillId = Int32.Parse(idBill);
            _ReturnOrder.WahoId = eSession.WahoId;
            if (string.IsNullOrEmpty(state))
            {
                _ReturnOrder.State = false;
            }
            _ReturnOrder.State = true;
            //  add return order and return return order id
            var jsonOrder = JsonConvert.SerializeObject(_ReturnOrder);
            var contentOrder = new StringContent(jsonOrder, Encoding.UTF8, "application/json");
            var responseOrder = await client.PostAsync(returnOrderAPIUrl, contentOrder);
            if ((int)responseOrder.StatusCode == 401) await HttpContext.SignOutAsync("CookieAuthentication");

            if (responseOrder.IsSuccessStatusCode)
            {
                var responseContent = await responseOrder.Content.ReadAsStringAsync();
                PostReturnOrderVM postOrderVM = JsonConvert.DeserializeObject<PostReturnOrderVM>(responseContent);
                foreach (var billDetail in billDetails)
                {
                    // Thiết lập giá trị BillId cho bản ghi BillDetail
                    billDetail.ReturnOrderId = postOrderVM.ReturnOrderId;
                }

                // Thêm bản ghi BillDetail vào context
                var jsonOrderProduct = JsonConvert.SerializeObject(billDetails);
                var contentOrderProduct = new StringContent(jsonOrderProduct, Encoding.UTF8, "application/json");
                var responseOrderProduct = await client.PostAsync($"{returnOrderAPIUrl}/detail", contentOrderProduct);
                if (!responseOrderProduct.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Tạo hoàn đơn thất bại do không add được return order product!";
                    return RedirectToPage("./Index");
                }
                //_context.ReturnOrderProducts.AddRange(billDetails);
                //await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Tạo hoàn đơn thành công!";
                return RedirectToPage("./Index");
            }
            //_context.ReturnOrders.Add(_ReturnOrder);
            //int result = _context.SaveChanges();
            TempData["ErrorMessage"] = "Tạo hoàn đơn thất bại!";
            return RedirectToPage("./Index");
        }
    }
}
