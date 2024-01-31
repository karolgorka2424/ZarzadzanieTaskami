using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ZarzadzanieTaskami.Data;
using ZarzadzanieTaskami.Models;
using System.Collections.Generic;
using System.Linq;

namespace ZarzadzanieTaskami
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
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
            app.MapRazorPages();

            // Create roles, users, and sample data
            using (var scope = app.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                await CreateRoles(serviceProvider);
                await CreateUsers(serviceProvider);
                await CreateSampleData(serviceProvider); // Dodano wywołanie metody CreateSampleData
            }

            app.Run();
        }

        public static async CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "Administrator", "Użytkownik" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        public static async System.Threading.Tasks.CreateUsers(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // Przykładowi użytkownicy
            var users = new List<(string Email, string Password, string Role)>
            {
                ("admin@example.com", "Admin123!", "Administrator"),
                ("user1@example.com", "User123!", "Użytkownik"),
                ("user2@example.com", "User123!", "Użytkownik"),
                ("user3@example.com", "User123!", "Użytkownik"),
                ("user4@example.com", "User123!", "Użytkownik")
            };

            foreach (var userTuple in users)
            {
                var user = await userManager.FindByEmailAsync(userTuple.Email);
                if (user == null)
                {
                    user = new IdentityUser { UserName = userTuple.Email, Email = userTuple.Email };
                    var result = await userManager.CreateAsync(user, userTuple.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, userTuple.Role);
                    }
                }
            }
        }
        public static async Task CreateSampleData(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Upewnij się, że baza danych istnieje i jest zaktualizowana do najnowszej migracji
                context.Database.EnsureCreated();

                // Dodaj przykładowe projekty, jeśli jeszcze nie istnieją
                if (!context.Projekty.Any())
                {
                    var projekty = new List<Projekt>
            {
                new Projekt { Nazwa = "Projekt Alpha" },
                new Projekt { Nazwa = "Projekt Beta" },
                // Dodaj więcej projektów według potrzeb
            };
                    context.Projekty.AddRange(projekty);
                    await context.SaveChangesAsync();
                }

                // Dodaj przykładowe taski, jeśli jeszcze nie istnieją
                if (!context.Taski.Any())
                {
                    var projektAlpha = context.Projekty.FirstOrDefault(p => p.Nazwa == "Projekt Alpha");
                    var projektBeta = context.Projekty.FirstOrDefault(p => p.Nazwa == "Projekt Beta");

                    var taski = new List<Task>
            {
                new Task { Opis = "Task 1 dla Projektu Alpha", ProjektId = projektAlpha?.ProjektId ?? 0, CzyZakonczony = false },
                new Task { Opis = "Task 2 dla Projektu Alpha", ProjektId = projektAlpha?.ProjektId ?? 0, CzyZakonczony = true },
                new Task { Opis = "Task 1 dla Projektu Beta", ProjektId = projektBeta?.ProjektId ?? 0, CzyZakonczony = false },
                // Dodaj więcej tasków według potrzeb
            };
                    context.Taski.AddRange(taski);
                    await context.SaveChangesAsync();
                }

                // Dodaj przykładowe komentarze, jeśli jeszcze nie istnieją
                if (!context.Komentarze.Any())
                {
                    var task = context.Taski.FirstOrDefault();

                    var komentarze = new List<Komentarz>
            {
                new Komentarz { Tresc = "To jest komentarz do taska", TaskId = task?.TaskId ?? 0 },
                // Dodaj więcej komentarzy według potrzeb
            };
                    context.Komentarze.AddRange(komentarze);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
