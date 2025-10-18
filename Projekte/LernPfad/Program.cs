using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LernPfad.Data;

namespace LernPfad
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Datenbank-Kontext registrieren
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Identity hinzufügen
            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>();

            // Razor Pages & MVC registrieren
            builder.Services.AddRazorPages(); // ⬅️ Wichtig für Login/Register
            builder.Services.AddControllersWithViews();

            /*builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.ListenAnyIP(7000); // HTTP
                serverOptions.ListenAnyIP(7001, listenOptions =>
                {
                    listenOptions.UseHttps(); // HTTPS
                });
            });*/

            var app = builder.Build();

            // HTTP-Pipeline konfigurieren
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // ⬅️ Auth aktivieren
            app.UseAuthorization();

            // Standardroute für MVC
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Lernpfad}/{action=Create}/{id?}");

            // Identity UI (Login, Register usw.)
            app.MapRazorPages();

            app.Run();
        }
    }
}
