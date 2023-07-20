using BusinessObjects.WahoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.BillRepository;
using Repositories.ReturnOrderRepository;
using System.Collections.Generic;
using ViewModels.BillDetailViewModels;
using ViewModels.BillViewModel;
using ViewModels.CustomerViewModels;
using ViewModels.OrderDetailViewModels;
using ViewModels.OrderViewModels;

namespace WahoAPI.Controllers.BillController
{
    [Route("waho/[controller]")]
    [ApiController]

    public class BillsController : ControllerBase
    {
        private IBillRepositories respository = new BillRepositories();
        [HttpGet("detailById")]
        [Authorize(Roles = "1,2")]

        public ActionResult<List<BillDetail>> getBillDetailById(int billId)
        {
            List<BillDetail> billDetails = respository.GetBillDetailById(billId);
            if (billDetails.Count == 0)
            {
                return NotFound();
            }
            return Ok(billDetails);
        }

        [HttpGet("detail")]
        [Authorize(Roles = "1,2")]

        public ActionResult<Bill> getBillById(int billId)
        {
            Bill bill = respository.getBillById(billId);
            if (bill == null)
            {
                return NotFound();
            }
            return Ok(bill);
        }

        [HttpGet("detailByIdAndProId")]
        [Authorize]

        public ActionResult<BillDetail> getdetailByIdAndProId(int billId, int productId)
        {
            BillDetail billDetail = respository.GetBillDetailByIdAndProID(billId, productId);
            if (billDetail == null)
            {
                return NotFound();
            }
            return Ok(billDetail);
        }

        [HttpGet("getBills")]
        [Authorize]

        public IActionResult Get(int pageIndex, int pageSize, string? textSearch, string? status, string? dateFrom, string? dateTo, string? active, int wahoId)
        {
            var bill = respository.GetBillsPagingAndFilter(pageIndex, pageSize, textSearch, status, dateFrom, dateTo, active, wahoId);

            if (bill == null)
            {
                return NotFound();
            }
            return Ok(bill);
        }
        [HttpGet("count")]
        [Authorize]

        public ActionResult<int> CountPagingCustomer(int pageIndex, int pageSize, string? textSearch, string? status, string? dateFrom, string? dateTo, string? active, int wahoId)
        {
            var total = respository.CountPagingBill(pageIndex, pageSize, textSearch, status, dateFrom, dateTo, active, wahoId);
            if (total == null)
            {
                return NotFound();
            }
            return Ok(total);
        }
        [HttpPost]
        [Authorize]

        public IActionResult saveBill(PostBill PostBill)
        {
            int new_billID = respository.saveBill(PostBill);
            BillIdVM BillIdVM = new BillIdVM();
            BillIdVM.BillId = new_billID;
            return Ok(BillIdVM);
        }
        [HttpPut]
        [Authorize]

        public ActionResult PutBill(PostBill PostBill)
        {
            respository.UpdateBill(PostBill);
            return Ok();
        }
        [HttpPost("details")]
        [Authorize]

        public IActionResult AddBillDetails([FromBody] List<BillDetailVM> billDetailsVM)
        {
            respository.AddListBillDetail(billDetailsVM);
            return Ok();
        }
    }
}
