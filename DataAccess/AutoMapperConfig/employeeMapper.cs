using AutoMapper;
using BusinessObjects.WahoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.EmployeeViewModels;

namespace DataAccess.AutoMapperConfig
{
    public class employeeMapper : Profile
    {
        public static IMapper Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PostEmployeeVM, Employee>();
            });

            return config.CreateMapper();
        }
        public static IMapper ConfigureEToEVM()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, PostEmployeeVM> ();
            });

            return config.CreateMapper();
        }
    }
}
