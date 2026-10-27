using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_NET_65_MVC02.DAL.Models
{
    public class Category : BaseEntity
    {
        [MaxLength(20)]
        public string Name { get; set; } = null!;
        public ICollection<Session> Sessions { get; set; } = null!;
    }
}
