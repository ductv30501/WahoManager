using AutoMapper;
using BusinessObjects.WahoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.CustomerViewModels;
using ViewModels.DashBoardViewModels;

namespace DataAccess.AutoMapperConfig
{
    public class WahoMapper : Profile
    {
        public static IMapper ConfigureVMtoM()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WahoPostVM, WahoInformation>();
            });

            return config.CreateMapper();
        }
        public static IMapper ConfigureMToVM()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WahoInformation, WahoPostVM>();
            });

            return config.CreateMapper();
        }
    }
}
