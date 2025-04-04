using Company.Web.BLL;
using Company.Web.BLL.Interfaces;
using Company.Web.BLL.Repositories;
using Company.Web.DAL.Data.Contexts;
using Company.Web.DAL.Models;
using Company.Web.PL.Helper;
using Company.Web.PL.Mapping;
using Company.Web.PL.Settings;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Company.Web.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
            }).AddGoogle(o =>
            {
                o.ClientId = builder.Configuration["Auth:Google:ClientId"];
                o.ClientSecret = builder.Configuration["Auth:Google:ClientSecret"];
            });
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            //builder.Services.AddScoped<IDepartmentRepository,DepartmentRepository>();
            //builder.Services.AddScoped<IEmployeeRepository,EmployeeRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddDbContext<CompanyContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            //builder.Services.AddAutoMapper(typeof(EmployeeProfile));
            builder.Services.AddAutoMapper(m => m.AddProfile(new EmployeeProfile()));
            builder.Services.AddIdentity<AppUser, IdentityRole>(option =>
            {
                option.Password.RequiredLength = 4;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireUppercase = false;
                option.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<CompanyContext>().AddDefaultTokenProviders();
            builder.Services.ConfigureApplicationCookie(option =>
                option.LoginPath = "/Account/SignIn"
            );

            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
            builder.Services.AddScoped<IMailServices, MailServices>();

            //builder.Services.AddAuthentication(o =>
            //{
            //    o.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
            //    o.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            //}).AddGoogle(o =>
            //{
            //    o.ClientId = builder.Configuration["Auth:Google:ClientId"];
            //    o.ClientSecret = builder.Configuration["Auth:Google:ClientSecret"];
            //});

            //builder.Services.AddAuthentication(o =>
            //{
            //    o.DefaultAuthenticateScheme = FacebookDefaults.AuthenticationScheme;
            //    o.DefaultChallengeScheme = FacebookDefaults.AuthenticationScheme;
            //}).AddFacebook(o =>
            //{
            //    o.ClientId = builder.Configuration["Auth:Facebook:ClientId"];
            //    o.ClientSecret = builder.Configuration["Auth:Facebook:ClientSecret"];
            //});


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
