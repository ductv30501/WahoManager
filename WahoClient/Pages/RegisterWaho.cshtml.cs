using BusinessObjects.WahoModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using ViewModels.DashBoardViewModels;

namespace WahoClient.Pages
{
    public class RegisterWahoModel : PageModel
    {
        private readonly HttpClient client = null;
        private string productAPIUrl = "";
        private string wahoAPIUrl = "";
        public RegisterWahoModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            productAPIUrl = "https://localhost:7019/waho/Products";
            wahoAPIUrl = "https://localhost:7019/waho";
        }
        [BindProperty]
        public WahoPostVM waho { get; set; }
        [BindProperty]
        public List<Category> categories { get; set; }
        [BindProperty]
        public string Message { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            HttpResponseMessage response = await client.GetAsync($"{productAPIUrl}/categories");
            string strData = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                categories = JsonConvert.DeserializeObject<List<Category>>(strData);
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            HttpResponseMessage response = await client.GetAsync($"{wahoAPIUrl}/byNameEmail?name={waho.WahoName}&email={waho.Email}");
            string strData = await response.Content.ReadAsStringAsync();
            WahoInformation information = new WahoInformation();
            if (response.IsSuccessStatusCode)
            {
                information = JsonConvert.DeserializeObject<WahoInformation>(strData);
            }
            else
            {
                information = null;
            }

            if (information == null)
            {
                waho.WahoId= 0;
                waho.Active = true;
                var json = JsonConvert.SerializeObject(waho);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var responseAdd = await client.PostAsync(wahoAPIUrl, content);
                TempData["successMessage"] = "Tạo mới kho hàng thành công, hãy tạo mới tài khoản để đăng nhập vào kho hàng";
                return Page();
            }
            TempData["message"] = "tài khoản đã tồn tại";
            return Page();
        }


    }
}

