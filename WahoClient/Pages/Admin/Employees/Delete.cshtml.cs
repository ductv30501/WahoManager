using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.WahoModels;
using Waho.DataService;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using AutoMapper;
using DataAccess.AutoMapperConfig;
using ViewModels.EmployeeViewModels;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Authentication;

namespace WahoClient.Pages.Admin.Employees
{
    [Authorize(Roles = "1")]

    public class DeleteModel : PageModel
    {
        private readonly HttpClient client = null;
        private string employeeAPIUrl = "";
        private readonly Author _author;
        private static readonly IMapper _mapper = employeeMapper.ConfigureEToEVM();
        public DeleteModel( Author author)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            employeeAPIUrl = "https://localhost:7019/waho/Employee";
            _author = author;
        }
        public string message { get; set; }
        public string successMessage { get; set; }
        [BindProperty]
        public Employee Employee { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            //author
            if (User.Identity?.IsAuthenticated == false)
            {
                return RedirectToPage("/accessDenied", new { message = "do bạn chưa đăng nhập" });
            }

            if (id == null )
            {
                return NotFound();
            }
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Request.Cookies["AccessToken"]);

            // get employee by username
            HttpResponseMessage responseEmployee = await client.GetAsync($"{employeeAPIUrl}/username?username={id}");
            if ((int)responseEmployee.StatusCode == 401) return RedirectToPage("/accessDenied");

            string strDataEmployee = await responseEmployee.Content.ReadAsStringAsync();
            Employee employee = JsonConvert.DeserializeObject<Employee>(strDataEmployee);
            PostEmployeeVM postEmployeeVM = new PostEmployeeVM();
            postEmployeeVM = _mapper.Map<PostEmployeeVM>(employee);
            if (employee != null)
            {
                postEmployeeVM.Active = false;
                //update to data
                var json = JsonConvert.SerializeObject(postEmployeeVM);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(employeeAPIUrl, content);
                if ((int)response.StatusCode == 401) return RedirectToPage("/accessDenied");

                string messageResponse = await response.Content.ReadAsStringAsync();
                // message
                successMessage = $"Vô hiệu hóa thành công tài khoản: {postEmployeeVM.UserName}";
                TempData["successMessage"] = successMessage;
                return RedirectToPage("./Index");
            }
            message = "không tìm thấy nhân viên";
            TempData["message"] = message;
            return RedirectToPage("./Index");
        }
    }
}
