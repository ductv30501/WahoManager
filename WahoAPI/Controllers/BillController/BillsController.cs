using BusinessObjects.WahoModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.BillRepository;
using Repositories.ReturnOrderRepository;
using System.Collections.Generic;

namespace WahoAPI.Controllers.BillController
{
    [Route("waho/[controller]")]
    [ApiController]
    public class BillsController : ControllerBase
    {
        private IBillRepositories respository = new BillRepositories();
        [HttpGet("detailById")]
        public ActionResult<List<BillDetail>> getBillDetailById(int billId)
        {
            List<BillDetail> billDetails = respository.GetBillDetailById(billId);
            if (billDetails.Count == 0)
            {
                return NotFound();
            }
            return Ok(billDetails);
        }
        [HttpGet("detailByIdAndProId")]
        public ActionResult<BillDetail> getdetailByIdAndProId(int billId, int productId)
        {
            BillDetail billDetail = respository.GetBillDetailByIdAndProID(billId,productId);
            if (billDetail == null)
            {
                return NotFound();
            }
            return Ok(billDetail);
        }
    }
}
