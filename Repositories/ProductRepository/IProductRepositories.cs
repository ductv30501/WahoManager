﻿using BusinessObjects.WahoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProductViewModels;

namespace Repositories.ProductRepository
{
    public interface IProductRepositories
    {
        List<SubCategory> GetSubCategories(int wahoId);
        List<Supplier> GetSuppliers(int wahoId);
        List<Location> GetLocations();
        List<Product> GetAllProduct(int pageIndex, int pageSize, string textSearch, int subCategoryID, int location, int priceFrom, int priceTo, string inventoryLevel, int wahoId, string supplierId);
        int CountProducts(string textSearch, int location, int priceTo, int priceFrom, string inventoryLevel, string supplierName, int subCategoryID, int wahoId);
        void SaveProduct(ProductViewModel productVM);
        void UpdateProduct(ProductViewModel productVM);
        Product GetProductById(int productId);
        List<Product> GetProductsByWahoId(int wahoId);
        List<Product> SearchProducts(string textSearch, int wahoId);
        void AddListProduct(List<ProductViewModel> products);
        List<Category> GetCategories();
    }
}
