using Microsoft.EntityFrameworkCore;
using Sistema_de_tickets.Serv;
using VentoryIN.Models;

namespace VentoryIN
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Inyección del contexto para la base de datos
            builder.Services.AddDbContext<VentoryInDbContext>(opt =>
                opt.UseSqlServer(
                    builder.Configuration.GetConnectionString("VentoryInDbConnection")
                )
            );

            // Manejador de memoria
            builder.Services.AddSession(options =>
            {
                // Los segundos en que queremos que permanezca el estado 
                options.IdleTimeout = TimeSpan.FromSeconds(3600);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Añade IWebHostEnvironment a los servicios
            builder.Services.AddSingleton<IWebHostEnvironment>(builder.Environment);

            // Registrar IHttpContextAccessor
            builder.Services.AddHttpContextAccessor();

            // Registrar IUserService y su implementación
            builder.Services.AddScoped<IUserService, UserService>();


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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
