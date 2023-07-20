using BusinessObjects.WahoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Noding;
using Repositories.OrderRepository;
using Repositories.ProductRepository;
using ViewModels.OrderDetailViewModels;
using ViewModels.OrderViewModels;

namespace WahoAPI.Controllers.OrderController
{
    [Route("waho/[controller]")]
    [ApiController]

    public class OrdersController : ControllerBase
    {
        private IOrderRepositories respository = new OrderRepositories();
        [HttpGet("count")]
        [Authorize]

        public ActionResult<int> countOrderPaging(string? textSearch, string active, string status, string? dateTo, string? dateFrom, string? estDateTo, string? estDateFrom, int wahoId)
        {
            int total = respository.GetOrderCount(textSearch, active, status, dateTo, dateFrom, estDateTo, estDateFrom, wahoId);
            if (total == null)
            {
                return NotFound();
            }
            return Ok(total);
        }
        [HttpGet("paging")]
        [Authorize]

        public ActionResult<List<Oder>> OrdersPaging(int pageIndex, int pageSize, string? textSearch, string status, string? dateFrom, string? estDateFrom, string? estDateTo, string? dateTo, string active, int wahoId)
        {
            List<Oder> Oders = respository.GetOrdersPaging(pageIndex, pageSize, textSearch, status, dateFrom, estDateFrom, estDateTo, dateTo, active, wahoId);
            if (Oders.Count == 0)
            {
                return NotFound();
            }
            return Ok(Oders);
        }
        [HttpGet("shipperList")]
        [Authorize]

        public ActionResult<List<Shipper>> shippersList(string? textSearch, int wahoId)
        {
            List<Shipper> Shippers = respository.GetListShipperSearch(textSearch,wahoId);
            if (Shippers.Count == 0)
            {
                return NotFound();
            }
            return Ok(Shippers);
        }
        [HttpGet("orderById")]
        [Authorize]

        public ActionResult<Oder> orderById(int orderId)
        {
            Oder oder = respository.GetOrderById(orderId);
            if (oder == null)
            {
                return NotFound();
            }
            return Ok(oder);
        }
        [HttpGet("orderDetailsById")]
        [Authorize]

        public ActionResult<List<OderDetail>> orderDetailsById(int orderId)
        {
            List<OderDetail> oderDetails = respository.GetOrderDetailById(orderId);
            if (oderDetails.Count == 0)
            {
                return NotFound();
            }
            return Ok(oderDetails);
        }
        [HttpPost]
        [Authorize]

        public IActionResult saveOrder(OrderVM orderVM)
        {
            int new_orderID = respository.SaveOrder(orderVM);
            PostOrderVM postOrderVM = new PostOrderVM();
            postOrderVM.OderId = new_orderID;
            return Ok(postOrderVM);
        }
        [HttpPut]
        [Authorize]

        public ActionResult PutOrder(OrderVM orderVM)
        {
            respository.UpdateOrder(orderVM);
            return Ok();
        }
        [HttpPost("details")]
        [Authorize]

        public IActionResult AddOrderDetails([FromBody] List<OrderDetailVM> orderDetailsVM)
        {
           respository.AddListOrderDetail(orderDetailsVM);
            return Ok();
        }
        [HttpGet("OrderDetailByIDProID")]
        [Authorize]

        public ActionResult<OderDetail> OrderDetailByIDProID(int orderId, int productId)
        {
            OderDetail oderDetail = respository.GetOrderDetailByIdAndProId(orderId,productId);
            if (oderDetail == null)
            {
                return NotFound();
            }
            return Ok(oderDetail);
        }
    }
}
