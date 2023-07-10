using BusinessObjects.WahoModels;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProductViewModels;

namespace Repositories.ProductRepository
{
    public class ProductRepositories : IProductRepositories
    {
        public void AddListProduct(List<ProductViewModel> products) => ProductDAO.AddListProduct(products);

        public int CountProducts(string textSearch, int location, int priceTo, int priceFrom, string inventoryLevel, string supplierName, int subCategoryID, int wahoId)
            => ProductDAO.CountProducts(textSearch,location,priceTo,priceFrom,inventoryLevel,supplierName,subCategoryID,wahoId);

        public List<Product> GetAllProduct(int pageIndex, int pageSize, string textSearch, int subCategoryID, int location, int priceFrom, int priceTo, string inventoryLevel, int wahoId, string supplierId)
            => ProductDAO.GetAllProduct(pageIndex,pageSize,textSearch,subCategoryID,location,priceFrom,priceTo,inventoryLevel,wahoId, supplierId);

        public List<Category> GetCategories()
            => ProductDAO.GetCategories();

        public List<Location> GetLocations() => ProductDAO.GetLocations();

        public Product GetProductById(int productId) => ProductDAO.getProductById(productId);

        public List<Product> GetProductsByWahoId(int wahoId) => ProductDAO.getProductsByWahoId(wahoId);

        public List<SubCategory> GetSubCategories(int wahoId) => ProductDAO.GetSubCategories(wahoId);

        public List<Supplier> GetSuppliers(int wahoId) => ProductDAO.GetSuppliers(wahoId);

        public void SaveProduct(ProductViewModel productVM) => ProductDAO.SaveProducts(productVM);

        public List<Product> SearchProducts(string textSearch, int wahoId) => ProductDAO.GetProductsSearch(textSearch, wahoId);

        public void UpdateProduct(ProductViewModel productVM) => ProductDAO.UpdateProducts(productVM);

    }
}
