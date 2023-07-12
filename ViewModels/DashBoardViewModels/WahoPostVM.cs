using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.DashBoardViewModels
{
    public class WahoPostVM
    {
        public int WahoId { get; set; }
        public string? WahoName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public bool? Active { get; set; }
        public int CategoryId { get; set; }
    }
}
