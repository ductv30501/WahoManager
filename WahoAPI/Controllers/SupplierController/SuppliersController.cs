using BusinessObjects.WahoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.SupplierRepository;
using ViewModels.SupplierViewModels;

namespace WahoAPI.Controllers.SupplierController
{
    [Route("waho/[controller]")]
    [ApiController]

    public class SuppliersController : ControllerBase
    {
        private ISupplierRepositories respository = new SupplierRepositories();
        [HttpPost]
        [Authorize]

        public IActionResult PostSupplier(SupplierVM supplierVM) {
            respository.addSupplier(supplierVM);
            return Ok();
        }
        [HttpPut]
        [Authorize]

        public IActionResult PutSupplier(SupplierVM supplierVM)
        {
            respository.updateSupplier(supplierVM);
            return Ok();
        }
        [HttpGet("getByID")]
        [Authorize]

        public ActionResult<Supplier> GetSupplierPagingAndFilter(int supId)
        {
            var supplier = respository.getsupplierByID(supId);
            if (supplier == null)
            {
                return NotFound();
            }
            return Ok(supplier);
        }
        [HttpGet("getSuppliers")]
        [Authorize]

        public ActionResult<List<Supplier>> GetSuppliers(int wahoId)
        {
            var suppliers = respository.GetSupplies(wahoId);
            if (suppliers == null)
            {
                return NotFound();
            }
            return Ok(suppliers);
        }
        [HttpGet("countPagingSupplier")]
        [Authorize]

        public ActionResult<int> countSuppliers(string? textSearch, int wahoId)
        {
            var total = respository.countSuppliers(textSearch, wahoId);
            if (total == null)
            {
                return NotFound();
            }
            return Ok(total);
        }
        [HttpGet("getSupplierPaging")]
        [Authorize]

        public ActionResult<List<Supplier>> GetSupplierPagingAndFilter(int pageIndex, int pageSize, string? textSearch, int wahoId)
        {
            var suppliers = respository.GetSupplierPagingAndFilter(pageIndex, pageSize, textSearch, wahoId);
            if (suppliers == null || suppliers.Count == 0)
            {
                return NotFound();
            }
            return Ok(suppliers);
        }
    }
}
