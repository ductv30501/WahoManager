using AutoMapper;
using BusinessObjects.WahoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ReturnOrderViewModels;

namespace DataAccess.AutoMapperConfig
{
    public class ReturnOrderProductMapper : Profile
    {
        public static IMapper ConfigureVMtoM()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReturnOrderProductVM, ReturnOrderProduct>();
            });

            return config.CreateMapper();
        }
        public static IMapper ConfigureMToVM()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReturnOrderProduct, ReturnOrderProductVM>();
            });

            return config.CreateMapper();
        }
    }
}
