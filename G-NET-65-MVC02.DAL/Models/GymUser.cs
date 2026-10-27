using G_NET_65_MVC02.DAL.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_NET_65_MVC02.DAL.Models
{
    public abstract class GymUser : BaseEntity
    {
        [Required,MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Email { get; set; } = null!;

        [Required, MaxLength(11)]
        [RegularExpression("^(010|011|012|015)\\d{8}$",ErrorMessage = "Egyption Phone Number Format Only!")]
        public string PhoneNumber { get; set; } = null!;

        public DateOnly DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public Address Address { get; set; } = null!;
    }

    [Owned]
    public class Address
    {
        public int BuildingNumber { get; set; }

        [Required, MaxLength(30)]
        public string Street { get; set; } = null!;

        [Required, MaxLength(30)]
        public string City { get; set; } = null!;
    }
}
