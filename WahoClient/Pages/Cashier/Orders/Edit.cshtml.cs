﻿using System;
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
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ViewModels.OrderDetailViewModels;
using System.Text;
using ViewModels.OrderViewModels;

namespace WahoClient.Pages.Cashier.Orders
{
    public class EditModel : PageModel
    {
        private readonly HttpClient client = null;
        private string orderAPIUrl = "";
        private readonly Author _author;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static readonly IMapper _mapper = OrderMapper.ConfigureMToVM();

        public EditModel(Author author, IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            orderAPIUrl = "https://localhost:7019/waho/Orders";
            _author = author;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public Oder Order { get; set; } = default!;

        public List<OderDetail> OrderDetails { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            //author
            if (!_author.IsAuthor(2))
            {
                return RedirectToPage("/accessDenied", new { message = "Thu Ngân" });
            }
            // get order by order id 
            HttpResponseMessage responseOder = await client.GetAsync($"{orderAPIUrl}/orderById?orderId={id}");
            string strDataOder = await responseOder.Content.ReadAsStringAsync();
            if (responseOder.IsSuccessStatusCode)
            {
                Order = JsonConvert.DeserializeObject<Oder>(strDataOder);
            }
            // get order details by order id
            HttpResponseMessage responseOderDe = await client.GetAsync($"{orderAPIUrl}/orderDetailsById?orderId={id}");
            string strDataOderDe = await responseOderDe.Content.ReadAsStringAsync();
            if (responseOderDe.IsSuccessStatusCode)
            {
                OrderDetails = JsonConvert.DeserializeObject<List<OderDetail>>(strDataOderDe);
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            // update order information
            OrderVM orderVM = _mapper.Map<OrderVM>(Order);
            var json = JsonConvert.SerializeObject(orderVM);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(orderAPIUrl, content);
            if (response.IsSuccessStatusCode)
            {
                TempData["successMessage"] = "Đã sửa thành công thông tin đơn vận";
                return RedirectToPage("./Index");
            }
            return RedirectToPage("./Index");
        }
    }
}
