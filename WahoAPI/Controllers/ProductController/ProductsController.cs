using BusinessObjects.WahoModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.ProductRepository;

namespace WahoAPI.Controllers.ProductController
{
    [Route("waho/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductRepositories respository = new ProductRepositories();
        [HttpGet("subcategories")]
        public ActionResult<List<SubCategory>> getsubcategories(int wahoId)
        {
            List<SubCategory> subCategories = respository.GetSubCategories(wahoId);
            if (subCategories == null)
            {
                return NotFound();
            }
            return Ok(subCategories);
        }
        [HttpGet("suppliers")]
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
        public ActionResult<List<Product>> GetProductPaging(int pageIndex, int pageSize, string? textSearch, int subCategoryID, int location, int priceFrom, int priceTo, string inventoryLevel, int wahoId, string supplierId)
        {
            List<Product> products = respository.GetAllProduct(pageIndex, pageSize, textSearch, subCategoryID, location, priceFrom, priceTo, inventoryLevel, wahoId, supplierId);
            if (products.Count == 0)
            {
                return NotFound();
            }
            return Ok(products);
        }
    }
}
