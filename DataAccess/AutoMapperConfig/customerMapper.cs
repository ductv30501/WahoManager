using AutoMapper;
using BusinessObjects.WahoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.CustomerViewModels;

namespace DataAccess.AutoMapperConfig
{

    public class customerMapper : Profile
    {
        public static IMapper ConfigureVMtoM()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PostCustomerVM, Customer>();
            });

            return config.CreateMapper();
        }
        public static IMapper ConfigureMtoVM()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Customer, PostCustomerVM>();
            });

            return config.CreateMapper();
        }
        public static IMapper ConfigureLMtoLVM()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<List<Customer>, List<PostCustomerVM>>();

            });

            return config.CreateMapper();
        }
    }
}
