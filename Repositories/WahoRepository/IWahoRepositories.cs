using BusinessObjects.WahoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.DashBoardViewModels;

namespace Repositories.WahoRepository
{
    public interface IWahoRepositories
    {
        List<WahoInformation> GetWaho();
        WahoInformation GetWahoByNameEmail(string name, string email);
        void SaveWaho(WahoPostVM wahoPostVM);
    }
}
