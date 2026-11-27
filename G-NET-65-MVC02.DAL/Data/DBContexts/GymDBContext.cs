using G_NET_65_MVC02.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace G_NET_65_MVC02.DAL.Data.DBContexts
{
    public class GymDBContext : IdentityDbContext<ApplicationUser>
    {
        public GymDBContext(DbContextOptions<GymDBContext> options) : base(options)
        {
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=.;DataBase=GymDB02;Trusted_Connection=True;TrustServerCertificate=True;");
        //}

        #region DBSets
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MemberShip> MemberShips { get; set; }
        public DbSet<Plan> Plans { get; set; } 
        public DbSet<Session> Sessions { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.ApplyConfiguration<Plan>(new Configurations.PlanConfiguration());
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Entity<ApplicationUser>(EB =>
            {
                EB.Property(x => x.FirstName)
                .HasColumnType("Varchar")
                .HasMaxLength(50);

                EB.Property(x => x.LastName)
                .HasColumnType("Varchar")
                .HasMaxLength(50);
            });
        }
    }
}
