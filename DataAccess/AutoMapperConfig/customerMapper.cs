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
    public class customerMapper:Profile
    {
        public static IMapper Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PostCustomerVM, Customer>();
            });

            return config.CreateMapper();
        }
        public static IMapper ConfigureCToCVM()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<List<Customer>, List<GetCustomerVM>>();
            });

            return config.CreateMapper();
        }
    }
}
