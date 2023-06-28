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
using System.Text;
using AutoMapper;
using DataAccess.AutoMapperConfig;
using ViewModels.ReturnOrderViewModels;

namespace WahoClient.Pages.Cashier.ReturnOrders
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient client = null;
        private string returnOrderAPIUrl = "";
        private readonly Author _author;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static readonly IMapper _mapper = ReturnOrderMapper.ConfigureMToVM();

        public DeleteModel(Author author, IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            returnOrderAPIUrl = "https://localhost:7019/waho/ReturnOrders";
            _author = author;
            _httpContextAccessor = httpContextAccessor;
        }
        public string message { get; set; }
        public string successMessage { get; set; }
        [BindProperty]
      public ReturnOrder ReturnOrder { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            //author
            if (!_author.IsAuthor(2))
            {
                return RedirectToPage("/accessDenied", new { message = "Thu Ngân" });
            }
            ReturnOrder _returnOrder = new ReturnOrder();
            HttpResponseMessage responseRO = await client.GetAsync($"{returnOrderAPIUrl}/ROByID?returnId={id}");
            string strDataRO = await responseRO.Content.ReadAsStringAsync();
            if (responseRO.IsSuccessStatusCode)
            {
                _returnOrder = JsonConvert.DeserializeObject<ReturnOrder>(strDataRO);
            }
            //var _returnOrder = await _context.ReturnOrders.FirstOrDefaultAsync(m => m.ReturnOrderId == id);
            if (_returnOrder != null)
            {
                ReturnOrder = _returnOrder;
                ReturnOrder.Active = false;
                ReturnOrderVM returnOrderVM = _mapper.Map<ReturnOrderVM>(ReturnOrder);
                // udpate active = false
                var json = JsonConvert.SerializeObject(returnOrderVM);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(returnOrderAPIUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    successMessage = "Xóa thành công phiếu hoàn đơn ra khỏi danh sách";
                    TempData["successMessage"] = successMessage;
                    return RedirectToPage("./Index");
                }
            }
            message = "không tìm thấy phiếu hoàn đơn";
            TempData["message"] = message;
            return RedirectToPage("./Index");
        }

    }
}
