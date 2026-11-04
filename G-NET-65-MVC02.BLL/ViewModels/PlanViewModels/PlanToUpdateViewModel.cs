using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_NET_65_MVC02.BLL.ViewModels.PlanViewModels
{
    public class PlanToUpdateViewModel
    {
        //[MaxLength(0, ErrorMessage = "Cannot change name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Duration is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Duration must be greater than 0")]
        [MaxLength(3)]
        public int DurationInDays { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string? Description { get; set; } = default!;
    }
}
