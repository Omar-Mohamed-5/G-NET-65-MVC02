using G_NET_65_MVC02.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_NET_65_MVC02.DAL.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Category> builder)
        {
            builder.Property(c => c.Name)
                .HasColumnType("varchar")
                .HasMaxLength(20);

            builder.HasData
                (
                    new Category { Id = 1, Name = "GeneralFitness" },
                    new Category { Id = 2, Name = "Yoga" },
                    new Category { Id = 3, Name = "Boxing" },
                    new Category { Id = 4, Name = "CrossFit" }
                );

            builder.HasMany(c => c.Sessions)
                .WithOne(s => s.Category)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
