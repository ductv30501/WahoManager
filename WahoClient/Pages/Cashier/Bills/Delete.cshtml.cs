using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.WahoModels;
using AutoMapper;
using DataAccess.AutoMapperConfig;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using ViewModels.OrderViewModels;
using Waho.DataService;
using ViewModels.BillViewModel;

namespace WahoClient.Pages.Cashier.Bills
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient client = null;
        private string billAPIUrl = "";
        private readonly Author _author;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static readonly IMapper _mapper = BillMapper.ConfigureMToVM();

        public DeleteModel(Author author, IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            billAPIUrl = "https://localhost:7019/waho/Bills";
            _author = author;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public Bill Bill { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            //author
            if (!_author.IsAuthor(2))
            {
                return RedirectToPage("/accessDenied", new { message = "Thu Ngân" });
            }

            if (id == null)
            {
                return NotFound();
            }
            // get order by order id 
            HttpResponseMessage responseBill = await client.GetAsync($"{billAPIUrl}/detail?billId={id}");
            string strDataBill = await responseBill.Content.ReadAsStringAsync();
            
            if (responseBill.IsSuccessStatusCode)
            {
                Bill = JsonConvert.DeserializeObject<Bill>(strDataBill);
                Bill.Active = false;
                PostBill billVM = _mapper.Map<PostBill>(Bill);
                var json = JsonConvert.SerializeObject(billVM);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(billAPIUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Xoá thành công hoá đơn!";
                    return RedirectToPage("./Index");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Không tìm thấy hoá đơn!";
            }
            return RedirectToPage("./Index"); ;
        }
    }
}
