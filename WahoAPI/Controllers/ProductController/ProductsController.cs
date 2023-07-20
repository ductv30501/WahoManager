using BusinessObjects.WahoModels;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.ProductRepository;
using ViewModels.EmployeeViewModels;
using ViewModels.ProductViewModels;

namespace WahoAPI.Controllers.ProductController
{
    [Route("waho/[controller]")]
    [ApiController]

    public class ProductsController : ControllerBase
    {
        private IProductRepositories respository = new ProductRepositories();
        [HttpGet("subcategories")]
        [Authorize]

        public ActionResult<List<SubCategory>> getsubcategories(int wahoId)
        {
            List<SubCategory> subCategories = respository.GetSubCategories(wahoId);
            if (subCategories == null)
            {
                return NotFound();
            }
            return Ok(subCategories);
        }
        [HttpGet("categories")]
        [AllowAnonymous]

        public ActionResult<List<Category>> getcategories()
        {
            List<Category> Categories = respository.GetCategories();
            if (Categories == null)
            {
                return NotFound();
            }
            return Ok(Categories);
        }
        [HttpGet("suppliers")]
        [Authorize]

        public ActionResult<List<Supplier>> getsuppliers(int wahoId)
        {
            List<Supplier> suppliers = respository.GetSuppliers(wahoId);
            if (suppliers == null)
            {
                return NotFound();
            }
            return Ok(suppliers);
        }
        [HttpGet("location")]
        [Authorize]

        public ActionResult<List<Location>> GetLocation()
        {
            List<Location> Locations = respository.GetLocations();
            if (Locations == null)
            {
                return NotFound();
            }
            return Ok(Locations);
        }
        [HttpGet("countProduct")]
        [Authorize]

        public ActionResult<int> countProductPaging(string? textSearch, int location, int priceTo, int priceFrom, string inventoryLevel, string supplierName, int subCategoryID, int wahoId)
        {
            int total = respository.CountProducts(textSearch, location, priceTo, priceFrom, inventoryLevel, supplierName, subCategoryID, wahoId);
            if (total == null)
            {
                return NotFound();
            }
            return Ok(total);
        }
        [HttpGet("getProductPaging")]
        [Authorize]

        public ActionResult<List<Product>> GetProductPaging(int pageIndex, int pageSize, string? textSearch, int subCategoryID, int location, int priceFrom, int priceTo, string inventoryLevel, int wahoId, string supplierId)
        {
            List<Product> products = respository.GetAllProduct(pageIndex, pageSize, textSearch, subCategoryID, location, priceFrom, priceTo, inventoryLevel, wahoId, supplierId);
            if (products.Count == 0)
            {
                return NotFound();
            }
            return Ok(products);
        }
        [HttpPost]
        [Authorize]

        public IActionResult PostProduct(ProductViewModel pvm)
        {
            respository.SaveProduct(pvm);
            return Ok();
        }
        [HttpPut]
        [Authorize]

        public IActionResult PutProduct(ProductViewModel pvm)
        {
            respository.UpdateProduct(pvm);
            return Ok();
        }
        [HttpGet("productId")]
        [Authorize]

        public ActionResult<Product> GetProductById(int productId)
        {
            Product product = respository.GetProductById(productId);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        [HttpGet("searchProducts")]
        [Authorize]

        public ActionResult<List<Product>> searchProducts(string? textSearch,int wahoId)
        {
            List<Product> products = respository.SearchProducts(textSearch,wahoId);
            if (products.Count == 0)
            {
                return NotFound();
            }
            return Ok(products);
        }
        [HttpGet("wahoId")]
        [Authorize]

        public ActionResult<List<Product>> GetProductsByWahoId(int wahoId)
        {
            List<Product> products = respository.GetProductsByWahoId(wahoId);
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }
        [HttpPost("addproducts")]
        [Authorize]

        public IActionResult AddProducts([FromBody] List<ProductViewModel> products)
        {
            try
            {
                respository.AddListProduct(products);
                return Ok();
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về mã lỗi hoặc thông báo lỗi phù hợp
                return StatusCode(500, "An error occurred while adding products: " + ex.Message);
            }
        }

    }
}
