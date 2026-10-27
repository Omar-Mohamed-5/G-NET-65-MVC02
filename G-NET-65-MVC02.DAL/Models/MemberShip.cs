using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_NET_65_MVC02.DAL.Models
{
    public class MemberShip : BaseEntity
    {
        public DateTime EndDate { get; set; }
        public int MemperId { get; set; }
        public Member Member { get; set; } = null!;
        public int PlanId { get; set; }
        public Plan Plan { get; set; } = null!;

        [NotMapped]
        public string status => EndDate > DateTime.UtcNow ? "Active" : "Expired";

        [NotMapped]
        public bool IsActive => EndDate > DateTime.UtcNow;
    }
}
