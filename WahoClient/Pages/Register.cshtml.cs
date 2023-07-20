using BusinessObjects.WahoModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using ViewModels.EmployeeViewModels;

namespace WahoClient.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly HttpClient client = null;
        private string employeeAPIUrl = "";
        private string wahoAPIUrl = "";
        public RegisterModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            employeeAPIUrl = "https://localhost:7019/waho/Employee";
            wahoAPIUrl = "https://localhost:7019/waho";
        }
        [BindProperty] public PostEmployeeVM Employee { get; set; }
        [BindProperty]
        public string Message { get; set; }
        [BindProperty]
        public List<WahoInformation> wahos { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            HttpResponseMessage responseWaho = await client.GetAsync(wahoAPIUrl);
            string strDataWaho = await responseWaho.Content.ReadAsStringAsync();
            wahos = JsonConvert.DeserializeObject<List<WahoInformation>>(strDataWaho);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            HttpResponseMessage response = await client.GetAsync(employeeAPIUrl + "/usernameAllWaho?username=" + Employee.UserName);
            string strData = await response.Content.ReadAsStringAsync();
            Employee _employee = new Employee();
            if (response.IsSuccessStatusCode)
            {
                _employee = JsonConvert.DeserializeObject<Employee>(strData);
            }
            else
            {
                _employee = null;
            }

            HttpResponseMessage responseList = await client.GetAsync(employeeAPIUrl + "/GetAll?wahoId=" + Employee.WahoId);
            string strDataList = await responseList.Content.ReadAsStringAsync();
            List<Employee> list = new List<Employee>();
            if (responseList.IsSuccessStatusCode)
            {
                list = JsonConvert.DeserializeObject<List<Employee>>(strDataList);
            }
            if (_employee == null)
            {
                if (list.Count == 0 || list == null)
                {
                    Employee.Active = true;
                    Employee.Role = 1;
                    //_context.Employees.Add(Employee);
                    //await _context.SaveChangesAsync();
                    var json = JsonConvert.SerializeObject(Employee);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var responseAdd = await client.PostAsync(employeeAPIUrl, content);
                    string messageResponse = await responseAdd.Content.ReadAsStringAsync();

                    //message
                    Message = $"{messageResponse}, hãy quay lại đăng nhập";
                    //Message = "tạo tài khoản thành công, hãy quay lại đăng nhập";
                    TempData["successMessage"] = Message;
                    return Page();
                }
                else
                {
                    Employee.Active = false;
                    //_context.Employees.Add(Employee);
                    //await _context.SaveChangesAsync();
                    var json = JsonConvert.SerializeObject(Employee);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var responseAdd = await client.PostAsync(employeeAPIUrl, content);
                    string messageResponse = await responseAdd.Content.ReadAsStringAsync();
                    Message = $"{messageResponse}, hãy liên hệ admin để được kích hoạt tài khoản";
                    //Message = "tạo tài khoản thành công, hãy liên hệ admin để được kích hoạt tài khoản";
                    TempData["successMessage"] = Message;
                    return Page();
                }
            }

            TempData["message"] = "tài khoản đã tồn tại";
            return Page();
        }
    }
}
