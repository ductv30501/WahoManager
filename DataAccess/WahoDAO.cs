using AutoMapper;
using BusinessObjects.WahoModels;
using DataAccess.AutoMapperConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ViewModels.DashBoardViewModels;

namespace DataAccess
{
    public class WahoDAO
    {
        private static readonly IMapper _mapper = WahoMapper.ConfigureVMtoM();
        public static List<WahoInformation> GetWaho()
        {
            try
            {
                using (var _context = new WahoS8Context())
                {
                    return _context.WahoInformations.Where(w => w.Active == true).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static WahoInformation GetWahoByNameEmail(string name, string email)
        {
            try
            {
                using (var _context = new WahoS8Context())
                {
                    return _context.WahoInformations.FirstOrDefault(w => w.WahoName == name && w.Email == email);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static void SaveWaho(WahoPostVM wahoPostVM)
        {
            WahoInformation wahoInformation = _mapper.Map<WahoInformation>(wahoPostVM);
            try
            {
                using (var _context = new WahoS8Context())
                {
                    _context.WahoInformations.Add(wahoInformation);
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
