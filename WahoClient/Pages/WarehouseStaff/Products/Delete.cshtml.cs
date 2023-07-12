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
using ViewModels.EmployeeViewModels;
using AutoMapper;
using DataAccess.AutoMapperConfig;
using ViewModels.ProductViewModels;

namespace WahoClient.Pages.WarehouseStaff.Products
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient client = null;
        private string productAPIUrl = "";
        private readonly Author _author;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static readonly IMapper _mapper = ProductConfigMapper.ConfigureMToVM();
        public DeleteModel(Author author, IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            productAPIUrl = "https://localhost:7019/waho/Products";
            _author = author;
            _httpContextAccessor = httpContextAccessor;
        }
        public string message { get; set; }
        public string successMessage { get; set; }
        [BindProperty]
      public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? productID)
        {
            if (!_author.IsAuthor(3))
            {
                return RedirectToPage("/accessDenied", new { message = "Quản lý sản phẩm" });
            }
            if (productID == null)
            {
                return NotFound();
            }
            // get product by id
            HttpResponseMessage responseProduct = await client.GetAsync($"{productAPIUrl}/productId?productId={productID}");
            string strDataProduct = await responseProduct.Content.ReadAsStringAsync();
            Product = JsonConvert.DeserializeObject<Product>(strDataProduct);
            ProductViewModel productVM = new ProductViewModel();
            productVM = _mapper.Map<ProductViewModel>(Product);
            if (Product != null)
            {
                productVM.Active = false;
                //update to data
                var json = JsonConvert.SerializeObject(productVM);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(productAPIUrl, content);
                // message
                if (response.IsSuccessStatusCode)
                {
                    successMessage = $"Xóa thành công sản phẩm {Product.ProductName}";
                    TempData["successMessage"] = successMessage;
                    return RedirectToPage("./Index");
                }
                message = $"Xóa sản phẩm {Product.ProductName} thất bại";
                TempData["message"] = message;
                return RedirectToPage("./Index");
            }
            return Page();
        }

    }
}
