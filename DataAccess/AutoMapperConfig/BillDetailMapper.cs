using AutoMapper;
using BusinessObjects.WahoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.BillDetailViewModels;
using ViewModels.OrderDetailViewModels;

namespace DataAccess.AutoMapperConfig
{
    public class BillDetailMapper : Profile
    {
        public static IMapper ConfigureVMtoM()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BillDetailVM, BillDetail>();
            });

            return config.CreateMapper();
        }
        public static IMapper ConfigureMToVM()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BillDetail, BillDetailVM>();
            });

            return config.CreateMapper();
        }
    }
}
