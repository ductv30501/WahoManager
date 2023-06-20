using AutoMapper;
using BusinessObjects.WahoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.OrderViewModels;
using ViewModels.ReturnOrderViewModels;

namespace DataAccess.AutoMapperConfig
{
    public class ReturnOrderMapper : Profile
    {
        public static IMapper ConfigureVMtoM()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReturnOrderVM, ReturnOrder>();
            });

            return config.CreateMapper();
        }
        public static IMapper ConfigureMToVM()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReturnOrder, ReturnOrderVM>();
            });

            return config.CreateMapper();
        }
    }
}
