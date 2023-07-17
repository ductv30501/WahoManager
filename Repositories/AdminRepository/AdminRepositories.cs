using BusinessObjects.WahoModels;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.DashBoardViewModels;

namespace Repositories.AdminRepository
{
    public class AdminRepositories : IAdminRepositories
    {
        public List<BillDetail> BillDetails(string date, int wahoID) => AdminDAO.BillDetails(date, wahoID);

        public List<ReturnOrder> ReturnOrdersInDay(int wahoID)
            => AdminDAO.ReturnOrdersInDay(wahoID);

        public int TotalBillInDay(int wahoID)
            => AdminDAO.TotalBillInDay(wahoID);

        public List<TotalMMVM> totalBillMMVMs(int month, int year, int wahoID)
            => AdminDAO.totalBillMMVMs(month, year, wahoID);

        public List<DayInMonth> totalNumberBillDayInMs(int month, int year, int wahoID) => AdminDAO.totalNumberBillDayInMs(month, year, wahoID);

        public List<TotalMMVM> totalNumberBillMs(int month, int year, int wahoID)
            => AdminDAO.totalNumberBillMs(month, year, wahoID);

        public List<DayInMonth> totalNumberOrdersDayInMs(int month, int year, int wahoID) => AdminDAO.totalNumberOrdersDayInMs(month, year, wahoID);

        public List<TotalMMVM> totalNummberOrdersMMVMs(int month, int year, int wahoID) => AdminDAO.totalNummberOrdersMMVMs(month, year, wahoID);

        public List<TotalMMVM> totalOrdersMMVMs(int month, int year, int wahoID) => AdminDAO.totalOrdersMMVMs(month, year, wahoID);

        public int TotalReturnInDay(int wahoID) => AdminDAO.TotalReturnInDay(wahoID);
    }
}
