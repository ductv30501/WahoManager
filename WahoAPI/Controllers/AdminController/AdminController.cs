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
        public ActionResult<List<BillDetail>> BillDetails(string date, int wahoID)
        {
            List<BillDetail> list = respository.BillDetails(date, wahoID);
            if (list.Count == 0)
            {
                return NotFound();
            }
            return Ok(list);
        }
        [HttpGet("ReturnOrdersInDay")]
        public ActionResult<List<ReturnOrder>> ReturnOrdersInDay(int wahoID)
        {
            List<ReturnOrder> list = respository.ReturnOrdersInDay(wahoID);
            if (list.Count == 0)
            {
                return NotFound();
            }
            return Ok(list);
        }
        [HttpGet("TotalBillInDay")]
        public ActionResult<int> TotalBillInDay(int wahoID)
        {
            int total = respository.TotalBillInDay(wahoID);
            return Ok(total);
        }
        [HttpGet("totalBillMMVMs")]
        public ActionResult<List<TotalMMVM>> totalBillMMVMs(int month, int year, int wahoID)
        {
            List<TotalMMVM> list = respository.totalBillMMVMs(month, year, wahoID);
            if (list.Count == 0)
            {
                return NotFound();
            }
            return Ok(list);
        }
        [HttpGet("totalNumberBillDayInMs")]
        public ActionResult<List<DayInMonth>> totalNumberBillDayInMs(int month, int year, int wahoID)
        {
            List<DayInMonth> list = respository.totalNumberBillDayInMs(month, year, wahoID);
            if (list.Count == 0)
            {
                return NotFound();
            }
            return Ok(list);
        }
        [HttpGet("totalNumberBillMs")]
        public ActionResult<List<TotalMMVM>> totalNumberBillMs(int month, int year, int wahoID)
        {
            List<TotalMMVM> list = respository.totalNumberBillMs(month, year, wahoID);
            if (list.Count == 0)
            {
                return NotFound();
            }
            return Ok(list);
        }
        [HttpGet("totalNumberOrdersDayInMs")]
        public ActionResult<List<DayInMonth>> totalNumberOrdersDayInMs(int month, int year, int wahoID)
        {
            List<DayInMonth> list = respository.totalNumberOrdersDayInMs(month, year, wahoID);
            if (list.Count == 0)
            {
                return NotFound();
            }
            return Ok(list);
        }
        [HttpGet("totalNummberOrdersMMVMs")]
        public ActionResult<List<TotalMMVM>> totalNummberOrdersMMVMs(int month, int year, int wahoID)
        {
            List<TotalMMVM> list = respository.totalNummberOrdersMMVMs(month, year, wahoID);
            if (list.Count == 0)
            {
                return NotFound();
            }
            return Ok(list);
        }
        [HttpGet("totalOrdersMMVMs")]
        public ActionResult<List<TotalMMVM>> totalOrdersMMVMs(int month, int year, int wahoID)
        {
            List<TotalMMVM> list = respository.totalOrdersMMVMs(month, year, wahoID);
            if (list.Count == 0)
            {
                return NotFound();
            }
            return Ok(list);
        }
        [HttpGet("TotalReturnInDay")]
        public ActionResult<int> TotalReturnInDay(int wahoID)
        {
            int total = respository.TotalReturnInDay(wahoID);
            return Ok(total);
        }
    }
}
