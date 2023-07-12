using AutoMapper;
using BusinessObjects.WahoModels;
using DataAccess.AutoMapperConfig;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.EmployeeViewModels;
using ViewModels.ProductViewModels;

namespace DataAccess
{
    public class ProductDAO
    {
        private static readonly IMapper _mapper = ProductConfigMapper.ConfigureVMtoM();
        public static List<Product> GetProductsSearch(string textSearch, int wahoId)
        {
            List<Product> products = new List<Product>();
            try
            {
                using (var _context = new WahoS8Context())
                {
                    var query = _context.Products.AsQueryable();
                    if (!string.IsNullOrEmpty(textSearch))
                    {
                        query = query.Where(p => p.ProductName.ToLower().Contains(textSearch.ToLower()) || p.ProductId.ToString().Contains(textSearch));
                    }
                    products = query.Where(p => p.Active == true && p.WahoId == wahoId)
                    .Where(p => p.Quantity > 0)
                    .Take(5).ToList();
                    return products;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static List<Category> GetCategories()
        {
            try
            {
                using (var _context = new WahoS8Context())
                {
                    return _context.Categories
                        .ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static List<SubCategory> GetSubCategories(int wahoId)
        {
            int categoryOfWaho = 0;
            try
            {
                using (var _context = new WahoS8Context())
                {
                    var waho = _context.WahoInformations.FirstOrDefault(w => w.WahoId == wahoId);
                    categoryOfWaho = waho.CategoryId;
                }
                using (var _context = new WahoS8Context())
                {
                    return _context.SubCategories
                        .Where(sb => sb.CategoryId == categoryOfWaho)
                        .ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static void SaveProducts(ProductViewModel productVM)
        {
            Product product = _mapper.Map<Product>(productVM);
            try
            {
                using (var _context = new WahoS8Context())
                {
                    _context.Products.Add(product);
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public static Product getProductById(int product_id)
        {
            Product product = new Product();
            try
            {
                using (var _context = new WahoS8Context())
                {

                    product = _context.Products.FirstOrDefault(p => p.ProductId == product_id);
                    return product;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public static void AddListProduct(List<ProductViewModel> productsVM)
        {
            List<Product> products = new List<Product>();
            foreach (var p in productsVM)
            {
                Product product = _mapper.Map<Product>(p);
                products.Add(product);
            }
            try
            {
                using (var _context = new WahoS8Context())
                {
                     _context.AddRange(products);
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static List<Product> getProductsByWahoId(int waho_id)
        {
            List<Product> products = new List<Product>();
            try
            {
                using (var _context = new WahoS8Context())
                {
                    products = _context.Products.Where(p => p.WahoId == waho_id)
                             .Where(p => p.Active == true)
                             .Include(p => p.SubCategory)
                             .Include(p => p.Supplier)
                             .Include(p => p.Location)
                             .ToList();
                    return products;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public static void UpdateProducts(ProductViewModel productVM)
        {
            Product product = _mapper.Map<Product>(productVM);
            try
            {
                using (var _context = new WahoS8Context())
                {
                    _context.Entry<Product>(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public static List<Supplier> GetSuppliers(int wahoId)
        {
            try
            {
                using (var _context = new WahoS8Context())
                {
                    return _context.Suppliers
                        .Where(sb => sb.WahoId == wahoId && sb.Active == true)
                        .ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public static List<Location> GetLocations()
        {
            try
            {
                using (var _context = new WahoS8Context())
                {
                    return _context.Locations
                        .ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public static List<Product> GetAllProduct(int pageIndex, int pageSize, string textSearch, int subCategoryID, int location, int priceFrom, int priceTo, string inventoryLevel, int wahoId, string supplierId)
        {
            List<Product> products = new List<Product>();
            int categoryOfWaho = 0;
            try
            {
                using (var _context = new WahoS8Context())
                {
                    var waho = _context.WahoInformations.FirstOrDefault(w => w.WahoId == wahoId);
                    categoryOfWaho = waho.CategoryId;
                }
                using (var _context = new WahoS8Context())
                {
                    var query = _context.Products.Include(p => p.SubCategory).ThenInclude(s => s.Category).Include(p => p.Supplier).Include(p => p.Location)
                                         .Where(p => p.SubCategory.CategoryId == categoryOfWaho && p.WahoId == wahoId)
                                         .Where(p => p.Active == true)
                                         .AsQueryable();
                    if (subCategoryID > 0)
                    {
                        query = query.Where(p => p.SubCategoryId == subCategoryID);
                    }
                    if (!string.IsNullOrEmpty(textSearch))
                    {
                        query = query.Where(p => p.ProductName.ToLower().Contains(textSearch.ToLower())
                                        || p.Trademark.ToLower().Contains(textSearch.ToLower())
                                        || p.Supplier.Branch.ToLower().Contains(textSearch.ToLower())
                                        || p.SubCategory.SubCategoryName.ToLower().Contains(textSearch.ToLower()));
                    }
                    //filter location
                    if (location > 0)
                    {
                        query = query.Where(p => p.LocationId == location);
                    }
                    //filter price range
                    if (priceTo > 0)
                    {
                        if (priceFrom > 0)
                        {
                            query = query.Where(p => p.UnitPrice >= priceFrom && p.UnitPrice <= priceTo);
                        }
                        else
                        {
                            query = query.Where(p => p.UnitPrice <= priceTo);
                        }
                    }
                    if (priceFrom > 0)
                    {
                        query = query.Where(p => p.UnitPrice >= priceFrom);
                    }
                    // filter inventory
                    if (inventoryLevel != "all")
                    {
                        if (inventoryLevel == "min")
                        {
                            query = query.Where(p => p.Quantity < p.InventoryLevelMin);
                        }
                        else
                        {
                            query = query.Where(p => p.Quantity > p.InventoryLevelMax);
                        }
                    }
                    //filter supplier
                    if (supplierId != "all")
                    {
                        int _supplierId = int.Parse(supplierId);
                        query = query.Where(p => p.SupplierId == _supplierId);
                    }
                    products = query.OrderBy(p => p.ProductName)
                            .Skip((pageIndex - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();
                }
                return products;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public static int CountProducts(string textSearch, int location, int priceTo, int priceFrom, string inventoryLevel, string supplierName, int subCategoryID, int wahoId)
        {
            int total = 0;
            try
            {
                using (var _context = new WahoS8Context())
                {
                    var query = _context.Products.Include(p => p.SubCategory).Include(p => p.Supplier).Where(p => p.WahoId == wahoId).Where(p => p.Active == true).AsQueryable();
                    if (!string.IsNullOrEmpty(textSearch))
                    {
                        query = query.Where(p => p.ProductName.ToLower().Contains(textSearch.ToLower())
                                || p.Trademark.ToLower().Contains(textSearch.ToLower())
                                || p.Supplier.Branch.ToLower().Contains(textSearch.ToLower())
                                || p.SubCategory.SubCategoryName.ToLower().Contains(textSearch.ToLower()));
                    }
                    if(subCategoryID > 0)
                    {
                        query = query.Where(p => p.SubCategoryId == subCategoryID);
                    }
                    // filter location
                    if (location > 0)
                    {
                        query = query.Where(p => p.LocationId == location);
                    }
                    // filter price range
                    if (priceTo > 0)
                    {
                        if (priceFrom > 0)
                        {
                            query = query.Where(p => p.UnitPrice >= priceFrom && p.UnitPrice <= priceTo);
                        }
                        else
                        {
                            query = query.Where(p => p.UnitPrice <= priceTo);
                        }
                    }
                    if (priceFrom > 0)
                    {
                        query = query.Where(p => p.UnitPrice >= priceFrom);
                    }
                    // filter inventory level
                    if (inventoryLevel != "all")
                    {
                        if (inventoryLevel == "min")
                        {
                            query = query.Where(p => p.Quantity < p.InventoryLevelMin);
                        }
                        else
                        {
                            query = query.Where(p => p.Quantity > p.InventoryLevelMax);
                        }
                    }
                    // supplier 
                    if (supplierName != "all")
                    {
                        query = query.Where(p => p.SupplierId == int.Parse(supplierName));
                    }
                    total = query.Count();
                }
                return total;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
    }
}
