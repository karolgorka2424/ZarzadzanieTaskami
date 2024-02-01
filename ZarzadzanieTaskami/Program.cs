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
                CreateRoles(serviceProvider);
                CreateUsers(serviceProvider);
                CreateSampleData(serviceProvider); // Dodano wywołanie metody CreateSampleData
            }

            app.Run();
        }

        public static void CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "Administrator", "Użytkownik" };
            foreach (var roleName in roleNames)
            {
                var roleExist = roleManager.RoleExistsAsync(roleName).Result;
                if (!roleExist)
                {
                    roleManager.CreateAsync(new IdentityRole(roleName)).Wait();
                }
            }
        }

        public static void CreateUsers(IServiceProvider serviceProvider)
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
                var user = userManager.FindByEmailAsync(userTuple.Email).Result;
                if (user == null)
                {
                    user = new IdentityUser { UserName = userTuple.Email, Email = userTuple.Email };
                    var result = userManager.CreateAsync(user, userTuple.Password).Result;
                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, userTuple.Role).Wait();
                    }
                }
            }
        }

        public static void CreateSampleData(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                context.Database.EnsureCreated();

                if (!context.Projekt.Any())
                {
                    var projekty = new List<Projekt>
            {
                new Projekt { Nazwa = "Projekt Alpha" },
                new Projekt { Nazwa = "Projekt Beta" },
            };
                    context.Projekt.AddRange(projekty);
                    context.SaveChanges();
                }

                if (!context.ProjectTask.Any())
                {
                    var projektAlpha = context.Projekt.FirstOrDefault(p => p.Nazwa == "Projekt Alpha");
                    var projektBeta = context.Projekt.FirstOrDefault(p => p.Nazwa == "Projekt Beta");

                    var taski = new List<ProjectTask>
            {
                new ProjectTask { Opis = "Task 1 dla Projektu Alpha", ProjektId = projektAlpha?.ProjektId ?? 0, CzyZakonczony = false },
                new ProjectTask { Opis = "Task 2 dla Projektu Alpha", ProjektId = projektAlpha?.ProjektId ?? 0, CzyZakonczony = true },
                new ProjectTask { Opis = "Task 1 dla Projektu Beta", ProjektId = projektBeta?.ProjektId ?? 0, CzyZakonczony = false },
            };
                    context.ProjectTask.AddRange(taski);
                    context.SaveChanges();
                }

                if (!context.Komentarz.Any())
                {
                    var taskDoKomentowania = context.ProjectTask.FirstOrDefault(t => !t.Komentarze.Any());

                    if (taskDoKomentowania != null)
                    {
                        var komentarze = new List<Komentarz>
                {
                    new Komentarz { Tresc = "To jest komentarz do taska", TaskId = taskDoKomentowania.TaskId },
                };
                        context.Komentarz.AddRange(komentarze);
                        context.SaveChanges();
                    }
                }

            }
        }

    }
}
