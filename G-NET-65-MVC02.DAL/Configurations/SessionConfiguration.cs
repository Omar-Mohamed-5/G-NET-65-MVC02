using G_NET_65_MVC02.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_NET_65_MVC02.DAL.Configurations
{
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(t =>
            {
                t.HasCheckConstraint("SessionCapacityConstraint", "Capacity Between 1  and 25");
                t.HasCheckConstraint("SessionEndDateAfterStartDate", "EndDate > StartDate");
            });

            builder.HasOne(s => s.Trainer)
                   .WithMany(t => t.TrainerSessions)
                   .HasForeignKey(s => s.TrainerId);

            builder.HasOne(s => s.Category)
                   .WithMany(c => c.Sessions)
                   .HasForeignKey(s => s.CategoryId);
        }
    }
}
