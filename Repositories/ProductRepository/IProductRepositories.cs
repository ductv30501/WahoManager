using BusinessObjects.WahoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ProductRepository
{
    public interface IProductRepositories
    {
        List<SubCategory> GetSubCategories(int wahoId);
        List<Supplier> GetSuppliers(int wahoId);
        List<Location> GetLocations();
        List<Product> GetAllProduct(int pageIndex, int pageSize, string textSearch, int subCategoryID, int location, int priceFrom, int priceTo, string inventoryLevel, int wahoId, string supplierId);
        int CountProducts(string textSearch, int location, int priceTo, int priceFrom, string inventoryLevel, string supplierName, int subCategoryID, int wahoId);

    }
}
