using AutoMapper;
using BusinessObjects.WahoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.CustomerViewModels;
using ViewModels.OrderDetailViewModels;

namespace DataAccess.AutoMapperConfig
{
    public class OrderDetailMapper : Profile
    {
        public static IMapper ConfigureVMtoM()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderDetailVM, OderDetail>();
            });

            return config.CreateMapper();
        }
        public static IMapper ConfigureMToVM()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OderDetail, OrderDetailVM>();
            });

            return config.CreateMapper();
        }
    }
}
