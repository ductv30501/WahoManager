using BusinessObjects.WahoModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.ProductRepository;
using Repositories.ReturnOrderRepository;
using ViewModels.OrderViewModels;
using ViewModels.ReturnOrderViewModels;

namespace WahoAPI.Controllers.ReturnOrderController
{
    [Route("waho/[controller]")]
    [ApiController]
    public class ReturnOrdersController : ControllerBase
    {
        private IReturnOrderRepositories respository = new ReturnOrderRepositories();
        [HttpGet("count")]
        public ActionResult<int> countReturnOrder(string? textSearch, string employeeID, string status, string? dateFrom, string? dateTo, int wahoId)
        {
            int count = respository.countReturnOrder(textSearch, employeeID, status, dateFrom, dateTo, wahoId);
            if(count == 0)
            {
                return NotFound();
            }
            return Ok(count);
        }
        [HttpGet("paging")]
        public ActionResult<List<ReturnOrder>> ReturnOrderPaging(int pageIndex, int pageSize, string? textSearch, string userName, string status, string? raw_dateFrom, string? raw_dateTo, int wahoId)
        {
            List<ReturnOrder> returnOrders = respository.returnOrdersPaging(pageIndex, pageSize, textSearch, userName, status, raw_dateFrom, raw_dateTo, wahoId);
            if (returnOrders.Count == null)
            {
                return NotFound();
            }
            return Ok(returnOrders);
        }
        [HttpGet("returnOrdersByBillId")]
        public ActionResult<List<ReturnOrder>> returnOrdersByBillId(int billId, int wahoId)
        {
            List<ReturnOrder> returnOrders = respository.returnOrdersByBillId(billId, wahoId);
            if (returnOrders.Count == null)
            {
                return NotFound();
            }
            return Ok(returnOrders);
        }
        [HttpGet("ROPByReturnID")]
        public ActionResult<List<ReturnOrderProduct>> ReturnOrderProductsByReturnID(int returnId)
        {
            List<ReturnOrderProduct> rOPs = respository.ReturnOrderProductsByReturnID(returnId);
            if (rOPs.Count == 0)
            {
                return NotFound();
            }
            return Ok(rOPs);
        }
        [HttpPost]
        public IActionResult saveReturnOrder(ReturnOrderVM returnOrderVM)
        {
            int new_return_orderID = respository.saveReturnOrder(returnOrderVM);
            PostReturnOrderVM postROVM = new PostReturnOrderVM();
            postROVM.ReturnOrderId = new_return_orderID;
            return Ok(postROVM);
        }
        [HttpPost("detail")]
        public IActionResult saveListReturnOrderProduct([FromBody] List<ReturnOrderProductVM> ROPs)
        {
            respository.saveListReturnOrderProduct(ROPs);
            return Ok();
        }
        [HttpGet("ROByID")]
        public ActionResult<ReturnOrder> GetReturnOrderByID(int returnId)
        {
            ReturnOrder ROder = respository.GetReturnOrderByID(returnId);
            if (ROder == null)
            {
                return NotFound();
            }
            return Ok(ROder);
        }
        [HttpGet("ROPSPaging")]
        public ActionResult<List<ReturnOrderProduct>> ReturnOrderProductsPaging(int pageIndex, int pageSize, int id)
        {
            List<ReturnOrderProduct> rOPs = respository.RTOProductsPaging(pageIndex, pageSize, id);
            if (rOPs.Count == 0)
            {
                return NotFound();
            }
            return Ok(rOPs);
        }
        [HttpPut]
        public IActionResult putReturnOrder(ReturnOrderVM returnOrderVM)
        {
            respository.UpdateReturnOrder(returnOrderVM);
            return Ok();
        }
    }
}
