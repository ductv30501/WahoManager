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
        public List<BillDetail> BillDetails(DateTime date) => AdminDAO.BillDetails(date);

        public List<ReturnOrder> ReturnOrdersInDay()
            => AdminDAO.ReturnOrdersInDay();

        public int TotalBillInDay()
            => AdminDAO.TotalBillInDay();

        public List<TotalMMVM> totalBillMMVMs(int month, int year)
            => AdminDAO.totalBillMMVMs(month, year);

        public List<DayInMonth> totalNumberBillDayInMs(int month, int year)
            => AdminDAO.totalNumberBillDayInMs(month, year);

        public List<TotalMMVM> totalNumberBillMs(int month, int year)
            => totalNumberBillMs(month, year);

        public List<DayInMonth> totalNumberOrdersDayInMs(int month, int year)
            => AdminDAO.totalNumberOrdersDayInMs(month, year);

        public List<TotalMMVM> totalNummberOrdersMMVMs(int month, int year)
            => AdminDAO.totalNummberOrdersMMVMs(month, year);

        public List<TotalMMVM> totalOrdersMMVMs(int month, int year)
            => AdminDAO.totalOrdersMMVMs(month, year);

        public int TotalReturnInDay()
            => AdminDAO.TotalReturnInDay();
    }
}
