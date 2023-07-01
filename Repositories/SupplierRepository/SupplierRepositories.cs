using BusinessObjects.WahoModels;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.SupplierViewModels;

namespace Repositories.SupplierRepository
{
    public class SupplierRepositories : ISupplierRepositories
    {
        public void addSupplier(SupplierVM supplierVM) => SupplierDAO.addSupplier(supplierVM);

        public int countSuppliers(string textSearch, int wahoId) => SupplierDAO.countSuppliers(textSearch, wahoId);

        public Supplier getsupplierByID(int id) => SupplierDAO.getsupplierByID(id);

        public List<Supplier> GetSupplierPagingAndFilter(int pageIndex, int pageSize, string textSearch, int wahoId)
            => SupplierDAO.GetSupplierPagingAndFilter(pageIndex, pageSize, textSearch, wahoId);

        public List<Supplier> GetSupplies(int wahoId) => SupplierDAO.GetSupplies(wahoId);

        public void updateSupplier(SupplierVM supplierVM) => SupplierDAO.updateSupplier(supplierVM);
    }
}
