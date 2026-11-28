using G_NET_65_MVC02.BLL;
using G_NET_65_MVC02.BLL.Services.Classes;
using G_NET_65_MVC02.BLL.Services.Interfaces;
using G_NET_65_MVC02.DAL.Data.DBContexts;
using G_NET_65_MVC02.DAL.Models;
using G_NET_65_MVC02.DAL.Repositories.Classes;
using G_NET_65_MVC02.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace G_NET_65_MVC02_PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            #region Services
            builder.Services.AddDbContext<GymDBContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(Config =>
            {
                Config.User.RequireUniqueEmail = true;
                Config.Lockout.MaxFailedAccessAttempts = 5;
                Config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
            }).AddEntityFrameworkStores<GymDBContext>().AddDefaultTokenProviders();
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });
            //builder.Services.AddScoped<IPlanRepository, MockRepository>();
            //builder.Services.AddScoped<IPlanRepository, PlanRepository>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<IPlanService, PlanService>();
            builder.Services.AddScoped<ITrainerService, TrainerService>();
            //UnitOfWork and AutoMapper
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfiles()));
            #endregion

            var app = builder.Build();
            await app.MigrateAndSeedDataAsync();


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {   
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
