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
        int TotalBillInDay();
        List<BillDetail> BillDetails(DateTime date);
        int TotalReturnInDay();
        List<ReturnOrder> ReturnOrdersInDay();
        List<TotalMMVM> totalBillMMVMs(int month, int year);
        List<TotalMMVM> totalOrdersMMVMs(int month, int year);
        List<TotalMMVM> totalNumberBillMs(int month, int year);
        List<TotalMMVM> totalNummberOrdersMMVMs(int month, int year);
        List<DayInMonth> totalNumberBillDayInMs(int month, int year);
        List<DayInMonth> totalNumberOrdersDayInMs(int month, int year);
    }
}
