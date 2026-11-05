using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_NET_65_MVC02.BLL.ViewModels.TrainerViewModels
{
    public class TrainerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string DateOfBirth { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Specialties { get; set; }
    }
}
