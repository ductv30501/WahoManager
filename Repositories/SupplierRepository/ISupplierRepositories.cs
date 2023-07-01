using BusinessObjects.WahoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.SupplierViewModels;

namespace Repositories.SupplierRepository
{
    public interface ISupplierRepositories
    {
        int countSuppliers(string textSearch, int wahoId);
        List<Supplier> GetSupplierPagingAndFilter(int pageIndex, int pageSize, string textSearch, int wahoId);
        void addSupplier(SupplierVM supplierVM);
        void updateSupplier(SupplierVM supplierVM);
        Supplier getsupplierByID(int id);
        List<Supplier> GetSupplies(int wahoId);
    }
}
