using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.WahoModels;
using AutoMapper;
using DataAccess.AutoMapperConfig;
using System.Net.Http.Headers;
using Waho.DataService;
using Newtonsoft.Json;
using System.Text;
using ViewModels.OrderDetailViewModels;
using ViewModels.OrderViewModels;
using ViewModels.BillViewModel;

namespace WahoClient.Pages.Cashier.Bills
{
    public class EditModel : PageModel
    {
        private readonly HttpClient client = null;
        private string billAPIUrl = "";
        private readonly Author _author;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static readonly IMapper _mapper = BillMapper.ConfigureMToVM();

        public EditModel(Author author, IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            billAPIUrl = "https://localhost:7019/waho/Bills";
            _author = author;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public Bill Bill { get; set; } = default!;
        public List<BillDetail> billDetails { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            //author
            if (!_author.IsAuthor(2))
            {
                return RedirectToPage("/accessDenied", new { message = "Thu Ngân" });
            }
            // get order by order id 
            HttpResponseMessage responseBill = await client.GetAsync($"{billAPIUrl}/detailByIdAndProId?billId={id}&productId=0");
            string strDataBill = await responseBill.Content.ReadAsStringAsync();
            if (responseBill.IsSuccessStatusCode)
            {
                Bill = JsonConvert.DeserializeObject<Bill>(strDataBill);
            }
            // get order details by order id
            HttpResponseMessage responseOderDe = await client.GetAsync($"{billAPIUrl}/detailById?billId={id}");
            string strDataOderDe = await responseOderDe.Content.ReadAsStringAsync();
            if (responseOderDe.IsSuccessStatusCode)
            {
                billDetails = JsonConvert.DeserializeObject<List<BillDetail>>(strDataOderDe);
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            // update order information
            PostBill postBill = _mapper.Map<PostBill>(Bill);
            var json = JsonConvert.SerializeObject(postBill);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(billAPIUrl, content);
            if (response.IsSuccessStatusCode)
            {
                TempData["successMessage"] = "Đã sửa thành công thông tin hoá đơn";
                return RedirectToPage("./Index");
            }
            return RedirectToPage("./Index");
        }
    }
}
