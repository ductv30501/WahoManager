using BusinessObjects.WahoModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.EmployeeViewModels;

namespace DataAccess
{
    public class ProductDAO
    {
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
