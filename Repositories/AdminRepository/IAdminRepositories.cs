using BusinessObjects.WahoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.DashBoardViewModels;

namespace Repositories.AdminRepository
{
    public interface IAdminRepositories
    {
        int TotalBillInDay(int wahoID);
        List<BillDetail> BillDetails(DateTime date , int wahoID);
        int TotalReturnInDay(int wahoID);
        List<ReturnOrder> ReturnOrdersInDay(int wahoID);
        List<TotalMMVM> totalBillMMVMs(int month, int year, int wahoID);
        List<TotalMMVM> totalOrdersMMVMs(int month, int year, int wahoID);
        List<TotalMMVM> totalNumberBillMs(int month, int year, int wahoID);
        List<TotalMMVM> totalNummberOrdersMMVMs(int month, int year, int wahoID);
        List<DayInMonth> totalNumberBillDayInMs(int month, int year, int wahoID);
        List<DayInMonth> totalNumberOrdersDayInMs(int month, int year, int wahoID);
    }
}
