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
    public class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(g => g.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsRequired();
            builder.HasIndex(g => g.Name)
                   .IsUnique();


            builder.Property(g => g.Email)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();
            builder.HasIndex(g => g.Email)
                   .IsUnique();


            builder.Property(g => g.PhoneNumber)
                .HasColumnType("varchar")
                .HasMaxLength(11)
                .IsRequired();


            builder.OwnsOne(g => g.Address, Address =>
            {
                Address.Property(a => a.Street)
                       .HasColumnType("varchar")
                       .HasMaxLength(30);

                Address.Property(a => a.City)
                       .HasColumnType("varchar")
                       .HasMaxLength(30);
            });


            builder.ToTable(t => 
            {
                t.HasCheckConstraint("GymUser_EmailCheck","Email Like '_%@_%._%'");
                t.HasCheckConstraint("GymUser_PhoneNumberCheck", "PhoneNumber Like '010%' OR PhoneNumber Like '011%' OR PhoneNumber Like '012%' OR PhoneNumber Like '015%'");
            });
        }
    }
}
