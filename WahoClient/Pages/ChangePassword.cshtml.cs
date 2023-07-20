using BusinessObjects.WahoModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using ViewModels.EmployeeViewModels;
using Waho.DataService;
using Waho.MailManager;

namespace WahoClient.Pages
{
    public class ChangePasswordModel : PageModel
    {
        private readonly EmailService _emailService;
        private readonly HttpClient client = null;
        private string employeeAPIUrl = "";
        public ChangePasswordModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            employeeAPIUrl = "https://localhost:7019/waho/Employee";
        }
        public string message { get; set; }
        [BindProperty]
        public Employee employee { get; set; }
        [BindProperty]
        public string newPassword { get; set; }
        [BindProperty]
        public string newPasswordConfirm { get; set; }

        public async Task OnGetAsync()
        {
            //employee = await
        }
        public async Task<IActionResult> OnPostAsync()
        {
            HttpResponseMessage response = await client.GetAsync(employeeAPIUrl + "/getPostEmployee?username=" + employee.UserName + "&password=" + employee.Password);
            string strData = await response.Content.ReadAsStringAsync();

            PostEmployeeVM _employee = JsonConvert.DeserializeObject<PostEmployeeVM>(strData);
            if (_employee == null)
            {
                ModelState.AddModelError("", "Sai tên đăng nhập hoặc mật khẩu ");
            }
            else
            {
                _employee.Password = newPasswordConfirm;
                var json = JsonConvert.SerializeObject(_employee);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var responseUpdate = await client.PutAsync(employeeAPIUrl, content);
                string messageResponse = await responseUpdate.Content.ReadAsStringAsync();
                message = "Đổi mật khẩu thành công";
                return Page();
            }
            ModelState.AddModelError("", "Sai tên đăng nhập hoặc mật khẩu");
            return Page();
        }
    }
}
