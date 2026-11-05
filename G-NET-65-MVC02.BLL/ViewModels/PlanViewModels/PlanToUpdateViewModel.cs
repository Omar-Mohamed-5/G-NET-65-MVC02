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
        //[MaxLength(1,ErrorMessage = "Cannot change name")]
        public string? Name { get; set; } = default!;

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; } = default!;  

        [Required(ErrorMessage = "Duration is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Duration must be greater than 0")]
        public int DurationInDays { get; set; } = default!;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Description must be between 2 and 150 characters")]
        public string? Description { get; set; } = default!;
    }
}
