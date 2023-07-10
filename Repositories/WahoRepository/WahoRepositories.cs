using BusinessObjects.WahoModels;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.DashBoardViewModels;

namespace Repositories.WahoRepository
{
    public class WahoRepositories : IWahoRepositories
    {
        public List<WahoInformation> GetWaho()
            => WahoDAO.GetWaho();

        public WahoInformation GetWahoByNameEmail(string name, string email)
            => WahoDAO.GetWahoByNameEmail(name, email);

        public void SaveWaho(WahoPostVM wahoPostVM)
            => WahoDAO.SaveWaho(wahoPostVM);
    }
}
