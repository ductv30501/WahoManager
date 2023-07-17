﻿using System;
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
using System.Text;
using ViewModels.EmployeeViewModels;
using ViewModels.CustomerViewModels;

namespace WahoClient.Pages.Admin.Customers
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient client = null;
        private string customerAPIUrl = "";
        private readonly Author _author;
        private static readonly IMapper _mapper = customerMapper.ConfigureMtoVM();
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteModel(Author author, IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            customerAPIUrl = "https://localhost:7019/waho/Customer";
            _author = author;
            _httpContextAccessor= httpContextAccessor;
        }
        public string message { get; set; }
        public string successMessage { get; set; }

        [BindProperty]
      public Customer Customer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            //author
            if (!_author.IsAuthor(1))
            {
                return RedirectToPage("/accessDenied", new { message = "Trình quản lý của Admin" });
            }

            // get data from session
            var employeeJson = _httpContextAccessor.HttpContext.Session.GetString("Employee");
            EmployeeVM employeeVM = JsonConvert.DeserializeObject<EmployeeVM>(employeeJson);

            // get employee by username
            HttpResponseMessage responseCustomer = await client.GetAsync($"{customerAPIUrl}/detail?id={id}&wahoId={employeeVM.WahoId}");
            string strDataCustomer = await responseCustomer.Content.ReadAsStringAsync();
            Customer customer = JsonConvert.DeserializeObject<Customer>(strDataCustomer);
            if (customer != null)
            {
                PostCustomerVM postCustomerVM = _mapper.Map<PostCustomerVM>(customer);
                postCustomerVM.Active = false;
                //update to data
                var json = JsonConvert.SerializeObject(postCustomerVM);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(customerAPIUrl, content);
                string messageResponse = await response.Content.ReadAsStringAsync();
                // message
                successMessage = $"Xoá thành công bản ghi của: {postCustomerVM.CustomerName}";
                TempData["successMessage"] = successMessage;
                return RedirectToPage("./Index");
            }
            message = "không tìm thấy khách hàng nào";
            TempData["message"] = message;
            return RedirectToPage("./Index");
        }
    }
}
