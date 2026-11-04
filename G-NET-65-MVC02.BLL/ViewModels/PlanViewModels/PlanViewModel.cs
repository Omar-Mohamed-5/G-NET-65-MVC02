using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_NET_65_MVC02.BLL.ViewModels.PlanViewModels
{
    public class PlanViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int DurationInDays { get; set; }
        public string Description { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
