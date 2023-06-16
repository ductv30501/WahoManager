using BusinessObjects.WahoModels;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ProductRepository
{
    public class ProductRepositories : IProductRepositories
    {
        public int CountProducts(string textSearch, int location, int priceTo, int priceFrom, string inventoryLevel, string supplierName, int subCategoryID, int wahoId)
            => ProductDAO.CountProducts(textSearch,location,priceTo,priceFrom,inventoryLevel,supplierName,subCategoryID,wahoId);

        public List<Product> GetAllProduct(int pageIndex, int pageSize, string textSearch, int subCategoryID, int location, int priceFrom, int priceTo, string inventoryLevel, int wahoId, string supplierId)
            => ProductDAO.GetAllProduct(pageIndex,pageSize,textSearch,subCategoryID,location,priceFrom,priceTo,inventoryLevel,wahoId, supplierId);

        public List<Location> GetLocations() => ProductDAO.GetLocations();

        public List<SubCategory> GetSubCategories(int wahoId) => ProductDAO.GetSubCategories(wahoId);

        public List<Supplier> GetSuppliers(int wahoId) => ProductDAO.GetSuppliers(wahoId);
    }
}
