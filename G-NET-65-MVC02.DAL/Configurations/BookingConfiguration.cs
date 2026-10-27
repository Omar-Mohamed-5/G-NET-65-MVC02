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
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.Ignore(b => b.Id);

            builder.Property(b => b.CreatedAt)
                .HasColumnName("BookingDate")
                .HasDefaultValueSql("GETDATE()");

            #region Relations
            builder.HasOne(b => b.Session)
                       .WithMany(s => s.SessionMembers)
                       .HasForeignKey(b => b.SessionId);

            builder.HasOne(b => b.Member)
                       .WithMany(m => m.MemberSessions)
                       .HasForeignKey(b => b.MemberId);

            builder.HasKey(b => new { b.MemberId, b. SessionId });
            #endregion
        }
    }
}
