using HomeBankingMindHub.Models;
using HomeBankingMinHub.Controllers;
using HomeBankingMinHub.Intefaces;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace HomeBankingMinHub
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            // Add DbContext to the container
            //builder.Services.AddDbContext<HomeBankingContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("HomeBankingConexion")));

            // Add service to the container
            //builder.Services.AddScoped<IClientRepository, ClientRepository>();


            //builder.Services.AddControllers();
            // Add controllers to the container.
            //builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

            builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            builder.Services.AddDbContext<HomeBankingContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("HomeBankingConexion")));

            builder.Services.AddScoped<IClientRepository, ClientRepository>();
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            //app.MapGet("/hola", () => "hola");
            
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<HomeBankingContext>();

                DBInitializer.Initialize(context);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllers();

            app.MapRazorPages();

            app.Run();
        }
    }
}
