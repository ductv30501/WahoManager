using BusinessObjects.WahoModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.AdminRepository;
using Repositories.BillRepository;
using System.Collections.Generic;
using ViewModels.DashBoardViewModels;

namespace WahoAPI.Controllers.AdminController
{
    [Route("waho/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private IAdminRepositories respository = new AdminRepositories();
        [HttpGet("BillDetails")]
        public ActionResult<List<BillDetail>> BillDetails(DateTime date)
        {
            List<BillDetail> list = respository.BillDetails(date);
            if (list.Count == 0)
            {
                return NotFound();
            }
            return Ok(list);
        }
        [HttpGet("ReturnOrdersInDay")]
        public ActionResult<List<ReturnOrder>> ReturnOrdersInDay()
        {
            List<ReturnOrder> list = respository.ReturnOrdersInDay();
            if (list.Count == 0)
            {
                return NotFound();
            }
            return Ok(list);
        }
        [HttpGet("TotalBillInDay")]
        public ActionResult<int> TotalBillInDay()
        {
            int total = respository.TotalBillInDay();
            return Ok(total);
        }
        [HttpGet("totalBillMMVMs")]
        public ActionResult<List<TotalMMVM>> totalBillMMVMs(int month, int year)
        {
            List<TotalMMVM> list = respository.totalBillMMVMs(month, year);
            if (list.Count == 0)
            {
                return NotFound();
            }
            return Ok(list);
        }
        [HttpGet("totalNumberBillDayInMs")]
        public ActionResult<List<DayInMonth>> totalNumberBillDayInMs(int month, int year)
        {
            List<DayInMonth> list = respository.totalNumberBillDayInMs(month, year);
            if (list.Count == 0)
            {
                return NotFound();
            }
            return Ok(list);
        }
        [HttpGet("totalNumberBillMs")]
        public ActionResult<List<TotalMMVM>> totalNumberBillMs(int month, int year)
        {
            List<TotalMMVM> list = respository.totalNumberBillMs(month, year);
            if (list.Count == 0)
            {
                return NotFound();
            }
            return Ok(list);
        }
        [HttpGet("totalNumberOrdersDayInMs")]
        public ActionResult<List<DayInMonth>> totalNumberOrdersDayInMs(int month, int year)
        {
            List<DayInMonth> list = respository.totalNumberOrdersDayInMs(month, year);
            if (list.Count == 0)
            {
                return NotFound();
            }
            return Ok(list);
        }
        [HttpGet("totalNummberOrdersMMVMs")]
        public ActionResult<List<TotalMMVM>> totalNummberOrdersMMVMs(int month, int year)
        {
            List<TotalMMVM> list = respository.totalNummberOrdersMMVMs(month, year);
            if (list.Count == 0)
            {
                return NotFound();
            }
            return Ok(list);
        }
        [HttpGet("totalOrdersMMVMs")]
        public ActionResult<List<TotalMMVM>> totalOrdersMMVMs(int month, int year)
        {
            List<TotalMMVM> list = respository.totalOrdersMMVMs(month, year);
            if (list.Count == 0)
            {
                return NotFound();
            }
            return Ok(list);
        }
        [HttpGet("TotalReturnInDay")]
        public ActionResult<int> TotalReturnInDay()
        {
            int total = respository.TotalReturnInDay();
            return Ok(total);
        }
    }
}
