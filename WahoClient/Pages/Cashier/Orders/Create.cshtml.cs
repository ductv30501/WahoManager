﻿using System;
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
using System.Text;
using ViewModels.EmployeeViewModels;
using Microsoft.AspNetCore.Http;
using ViewModels.InventorySheetViewModels;
using ViewModels.CustomerViewModels;
using ViewModels.OrderViewModels;
using ViewModels.OrderDetailViewModels;
using AutoMapper;
using DataAccess.AutoMapperConfig;
using ViewModels.ProductViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Authentication;

namespace WahoClient.Pages.Cashier.Orders
{
    [Authorize(Roles = "1,2")]

    public class CreateModel : PageModel
    {
        private readonly HttpClient client = null;
        private string orderAPIUrl = "";
        private string productAPIUrl = "";
        private string CustomerAPIUrl = "";
        private readonly Author _author;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static readonly IMapper _mapper = OrderDetailMapper.ConfigureMToVM();
        private static readonly IMapper _mapperPro = ProductConfigMapper.ConfigureMToVM();

        public CreateModel(Author author, IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            orderAPIUrl = "https://localhost:7019/waho/Orders";
            productAPIUrl = "https://localhost:7019/waho/Products";
            CustomerAPIUrl = "https://localhost:7019/waho/Customers";
            _author = author;
            _httpContextAccessor = httpContextAccessor;
        }
        [BindProperty]
        public CustomerVM Customer { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public OderDetail OrderDetail { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<Product> products { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<Shipper> Shippers { get; set; }

        [BindProperty]
        public OrderVM Order { get; set; }

        private Employee employee { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            //author
            if (User.Identity?.IsAuthenticated == false)
            {
                return RedirectToPage("/accessDenied", new { message = "do bạn chưa đăng nhập" });
            }
            
            return Page();
        }

        public async Task<IActionResult> OnGetShippers(string? q)
        {
            if (string.IsNullOrWhiteSpace(q) || string.IsNullOrEmpty(q))
            {
                return new JsonResult("");
            }
            else
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Request.Cookies["AccessToken"]);

                // get data from session
                string employeeJson = _httpContextAccessor.HttpContext.Session.GetString("Employee");
                EmployeeVM eSession = JsonConvert.DeserializeObject<EmployeeVM>(employeeJson);
                // get list shipper
                HttpResponseMessage responseShiper = await client.GetAsync($"{orderAPIUrl}/shipperList?textSearch={q}&wahoId={eSession.WahoId}");
                if ((int)responseShiper.StatusCode == 401) return RedirectToPage("/accessDenied");

                string strDataShipper = await responseShiper.Content.ReadAsStringAsync();
                if (responseShiper.IsSuccessStatusCode)
                {
                    Shippers = JsonConvert.DeserializeObject<List<Shipper>>(strDataShipper);
                }
                return new JsonResult(Shippers);
            }
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Request.Cookies["AccessToken"]);

            string customerId = HttpContext.Request.Form["customerId"];
            string estDate = HttpContext.Request.Form["estDate"];
            string cod = HttpContext.Request.Form["cod"];
            string region = HttpContext.Request.Form["region"];
            string shipperId = HttpContext.Request.Form["shipperId"];
            string payed = HttpContext.Request.Form["payed"];
            string total = HttpContext.Request.Form["total"];
            string listOrderDetail = HttpContext.Request.Form["listOrderDetail"];
            List<OderDetail> orderDetails = JsonConvert.DeserializeObject<List<OderDetail>>(listOrderDetail);

            // get data from session
            string employeeJson = _httpContextAccessor.HttpContext.Session.GetString("Employee");
            EmployeeVM eSession = JsonConvert.DeserializeObject<EmployeeVM>(employeeJson);

            if (!string.IsNullOrEmpty(customerId))
            {
                Order.CustomerId = int.Parse(customerId);
            }
            else
            {
                string name = HttpContext.Request.Form["name"];
                string phone = HttpContext.Request.Form["phone"];
                string dob = HttpContext.Request.Form["dob"];
                string email = HttpContext.Request.Form["email"];
                string type = HttpContext.Request.Form["type"];
                string tax = HttpContext.Request.Form["tax"];
                string address = HttpContext.Request.Form["address"];
                string description = HttpContext.Request.Form["description"];

                Customer.CustomerName = name;
                Customer.Adress = address;
                Customer.Description = description;
                Customer.Email = email;
                Customer.Phone = phone;
                Customer.Dob = DateTime.Parse(dob);
                Customer.TypeOfCustomer = bool.Parse(type);
                Customer.TaxCode = tax;
                Customer.Active = true;
                Customer.WahoId = eSession.WahoId;

                // add new customer and return customer id
                var json = JsonConvert.SerializeObject(Customer);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(CustomerAPIUrl, content);
                if ((int)response.StatusCode == 401) return RedirectToPage("/accessDenied");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    PostCustomerVM postCusVM = JsonConvert.DeserializeObject<PostCustomerVM>(responseContent);
                    Order.CustomerId = postCusVM.CustomerId;
                }
            }

            Order.ShipperId = int.Parse(shipperId);
            Order.UserName = eSession.UserName;
            Order.OrderDate = DateTime.Now;
            Order.Active = true;
            Order.OderState = "notDelivery";
            Order.Total = decimal.Parse(total);
            if (!string.IsNullOrEmpty(payed))
            {
                Order.Deposit = decimal.Parse(payed);
            }
            Order.Cod = cod;
            Order.EstimatedDate = DateTime.Parse(estDate);
            Order.Region = region;
            Order.WahoId = eSession.WahoId;

            // add new order and return order id
            var jsonOrder = JsonConvert.SerializeObject(Order);
            var contentOrder = new StringContent(jsonOrder, Encoding.UTF8, "application/json");
            var responseOrder = await client.PostAsync(orderAPIUrl, contentOrder);
            if ((int)responseOrder.StatusCode == 401) return RedirectToPage("/accessDenied");

            List<OrderDetailVM> orderDetailsVM = new List<OrderDetailVM>();
            if (responseOrder.IsSuccessStatusCode)
            {
                var responseContent = await responseOrder.Content.ReadAsStringAsync();
                PostOrderVM postOrderVM = JsonConvert.DeserializeObject<PostOrderVM>(responseContent);
                foreach (var orderDetail in orderDetails)
                {
                    // get product to update quantity
                    HttpResponseMessage responsePro = await client.GetAsync($"{productAPIUrl}/productId?productId={orderDetail.ProductId}");
                    if ((int)responsePro.StatusCode == 401) return RedirectToPage("/accessDenied");

                    string strDataPro = await responsePro.Content.ReadAsStringAsync();
                    Product product = new Product();
                    if (responsePro.IsSuccessStatusCode)
                    {
                        product = JsonConvert.DeserializeObject<Product>(strDataPro);
                        product.Quantity -= orderDetail.Quantity;
                        ProductViewModel ProductVM = _mapperPro.Map<ProductViewModel>(product);
                        //update data
                        var jsonProUp = JsonConvert.SerializeObject(ProductVM);
                        var contentProUp = new StringContent(jsonProUp, Encoding.UTF8, "application/json");
                        var responseProUp = await client.PutAsync(productAPIUrl, contentProUp);
                        if ((int)responseProUp.StatusCode == 401) return RedirectToPage("/accessDenied");

                    }
                    // Thiết lập giá trị BillId cho bản ghi BillDetail
                    orderDetail.OderId = postOrderVM.OderId;
                    OrderDetailVM orderDetailVM = _mapper.Map<OrderDetailVM>(orderDetail);
                    orderDetailsVM.Add(orderDetailVM);
                }
                var jsonOrderDe = JsonConvert.SerializeObject(orderDetailsVM);
                var contentOrderDe = new StringContent(jsonOrderDe, Encoding.UTF8, "application/json");
                var responseOrderDe = await client.PostAsync($"{orderAPIUrl}/details", contentOrderDe);
                if ((int)responseOrderDe.StatusCode == 401) return RedirectToPage("/accessDenied");

                if (responseOrderDe.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Tạo vận đơn thành công!";
                    return RedirectToPage("./Index");
                }
            }
            TempData["ErrorMessage"] = "Tạo vận đơn thất bại";
            return RedirectToPage("./Index");
        }
    }
}
