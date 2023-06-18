using AutoMapper;
using BusinessObjects.WahoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.InventorySheetViewModels;

namespace DataAccess.AutoMapperConfig
{
    public class InventorySheetDetailMapper
    {
        public static IMapper ConfigureVMtoM()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<InventorySheetDetailVM, InventorySheetDetail>();
            });

            return config.CreateMapper();
        }
        public static IMapper ConfigureMToVM()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<InventorySheetDetail, InventorySheetDetailVM>();
            });

            return config.CreateMapper();
        }
    }
}
