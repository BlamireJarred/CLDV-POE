using Microsoft.EntityFrameworkCore;
using POEPart1_VenueBookingSystem.Models;
using POEPart1_VenueBookingSystem.Services; // Add this using statement

namespace POEPart1_VenueBookingSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register your database context
            builder.Services.AddDbContext<AppDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
                //sqlOptions => sqlOptions.EnableRetryOnFailure()
                ));

            // Register AzureStorageService
            builder.Services.AddScoped<AzureStorageService>(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("AzureBlobStorage");

                // Fallback to development storage if no connection string is found
                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = "UseDevelopmentStorage=true"; // For local development
                }

                return new AzureStorageService(connectionString);
            });

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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Booking}/{action=Index}/{id?}");

            app.Run();
        }
    }
}