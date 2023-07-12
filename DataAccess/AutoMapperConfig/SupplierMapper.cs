using AutoMapper;
using BusinessObjects.WahoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.EmployeeViewModels;
using ViewModels.SupplierViewModels;

namespace DataAccess.AutoMapperConfig
{
    public class SupplierMapper : Profile
    {
        public static IMapper ConfigureVMToM()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SupplierVM, Supplier>();
            });

            return config.CreateMapper();
        }
        public static IMapper ConfigureMToVM()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Supplier, SupplierVM>();
            });

            return config.CreateMapper();
        }
    }
}
