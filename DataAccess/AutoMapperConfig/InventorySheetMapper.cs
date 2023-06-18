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
    public class InventorySheetMapper
    {
        public static IMapper ConfigureVMtoM()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<InventorySheetVM, InventorySheet>();
            });

            return config.CreateMapper();
        }
        public static IMapper ConfigureMToVM()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<InventorySheet, InventorySheetVM>();
            });

            return config.CreateMapper();
        }
    }
}
