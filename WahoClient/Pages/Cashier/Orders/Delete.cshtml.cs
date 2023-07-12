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
using System.Net.Http.Headers;
using Waho.DataService;
using Newtonsoft.Json;
using ViewModels.OrderViewModels;
using System.Text;

namespace WahoClient.Pages.Cashier.Orders
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient client = null;
        private string orderAPIUrl = "";
        private readonly Author _author;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static readonly IMapper _mapper = OrderMapper.ConfigureMToVM();

        public DeleteModel(Author author, IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            orderAPIUrl = "https://localhost:7019/waho/Orders";
            _author = author;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public Oder Order { get; set; }

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
            HttpResponseMessage responseOder = await client.GetAsync($"{orderAPIUrl}/orderById?orderId={id}");
            string strDataOder = await responseOder.Content.ReadAsStringAsync();
            if (responseOder.IsSuccessStatusCode)
            {
                Order = JsonConvert.DeserializeObject<Oder>(strDataOder);
                Order.Active = false;
                OrderVM orderVM = _mapper.Map<OrderVM>(Order);
                var json = JsonConvert.SerializeObject(orderVM);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(orderAPIUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Xoá thành công vận đơn!";
                    return RedirectToPage("./Index");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Không tìm thấy vận đơn!";
            }
            return RedirectToPage("./Index"); ;
        }
       
    }
}
