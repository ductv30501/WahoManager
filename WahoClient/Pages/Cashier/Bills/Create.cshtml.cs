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
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using ViewModels.CustomerViewModels;
using ViewModels.OrderDetailViewModels;
using ViewModels.EmployeeViewModels;
using ViewModels.OrderViewModels;
using ViewModels.BillViewModel;
using ViewModels.BillDetailViewModels;
using ViewModels.ProductViewModels;

namespace WahoClient.Pages.Cashier.Bills
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient client = null;
        private string billAPIUrl = "";
        private string productAPIUrl = "";
        private string CustomerAPIUrl = "";
        private readonly Author _author;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static readonly IMapper _mapper = BillDetailMapper.ConfigureMToVM();
        private static readonly IMapper _mapperPro = ProductConfigMapper.ConfigureMToVM();

        public CreateModel(Author author, IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            billAPIUrl = "https://localhost:7019/waho/Bills";
            productAPIUrl = "https://localhost:7019/waho/Products";
            CustomerAPIUrl = "https://localhost:7019/waho/Customers";
            _author = author;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public Customer Customer { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public List<BillDetail> billDetails { get; set; }

        [BindProperty(SupportsGet = true)]
        public BillDetail billDetail { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<Product> products { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<Customer> customers { get; set; }

        [BindProperty]
        public PostBill Bill { get; set; }

        private Employee employee { get; set; }
        public IActionResult OnGet()
        {
            if (!_author.IsAuthor(2))
            {
                return RedirectToPage("/accessDenied", new { message = "Thu Ngân" });
            }
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            string customerId = HttpContext.Request.Form["customerId"];
            string total = HttpContext.Request.Form["total"];
            string listBillDetail = HttpContext.Request.Form["listBillDetail"];
            List<BillDetail> billDetails = JsonConvert.DeserializeObject<List<BillDetail>>(listBillDetail);

            // get data from session
            string employeeJson = _httpContextAccessor.HttpContext.Session.GetString("Employee");
            EmployeeVM eSession = JsonConvert.DeserializeObject<EmployeeVM>(employeeJson);

            if (!string.IsNullOrEmpty(customerId))
            {
                Bill.CustomerId = int.Parse(customerId);
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
                
                // add new customer and return customer id
                var json = JsonConvert.SerializeObject(Customer);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(CustomerAPIUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    PostCustomerVM postCusVM = JsonConvert.DeserializeObject<PostCustomerVM>(responseContent);
                    Bill.CustomerId = postCusVM.CustomerId;
                }
            }

            Bill.UserName = eSession.UserName;
            Bill.Date = DateTime.Now;
            Bill.Active = true;
            Bill.BillStatus = "done";
            Bill.Total = decimal.Parse(total);
            Bill.WahoId = eSession.WahoId;

            //add new Bill
            var jsonBill = JsonConvert.SerializeObject(Bill);
            var contentBill = new StringContent(jsonBill, Encoding.UTF8, "application/json");
            var responseBill = await client.PostAsync(billAPIUrl, contentBill);
            List<BillDetailVM> billDetailsVM = new List<BillDetailVM>();
            if (responseBill.IsSuccessStatusCode)
            {
                var responseContent = await responseBill.Content.ReadAsStringAsync();
                BillIdVM BillIdVM = JsonConvert.DeserializeObject<BillIdVM>(responseContent);
                foreach (var billDetail in billDetails)
                {
                    // get product to update quantity
                    HttpResponseMessage responsePro = await client.GetAsync($"{productAPIUrl}/productId?productId={billDetail.ProductId}");
                    string strDataPro = await responsePro.Content.ReadAsStringAsync();
                    Product product = new Product();
                    if (responsePro.IsSuccessStatusCode)
                    {
                        product = JsonConvert.DeserializeObject<Product>(strDataPro);
                        product.Quantity -= billDetail.Quantity;
                        ProductViewModel ProductVM = _mapperPro.Map<ProductViewModel>(product);
                        //update data
                        var jsonProUp = JsonConvert.SerializeObject(ProductVM);
                        var contentProUp = new StringContent(jsonProUp, Encoding.UTF8, "application/json");
                        HttpResponseMessage responseProUp = await client.PutAsync(productAPIUrl, contentProUp);
                        if (responseProUp.IsSuccessStatusCode)
                        {
                            // Thiết lập giá trị BillId cho bản ghi BillDetail
                            billDetail.BillId = BillIdVM.BillId;
                            BillDetailVM billDetailVM = _mapper.Map<BillDetailVM>(billDetail);
                            billDetailsVM.Add(billDetailVM);
                        }
                    }
                }
                var jsonBillDe = JsonConvert.SerializeObject(billDetailsVM);
                var contentBillDe = new StringContent(jsonBillDe, Encoding.UTF8, "application/json");
                var responseBillDe = await client.PostAsync($"{billAPIUrl}/details", contentBillDe);
                if (responseBillDe.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Tạo hoá đơn thành công!";
                    return RedirectToPage("./Index");
                }
            }
            TempData["ErrorMessage"] = "Tạo hoá đơn thất bại";
            return RedirectToPage("./Index");
        }
    }
}
