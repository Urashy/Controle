
using Api.Managers;
using Api.Models;
using Api.Models.EntityFramework;
using Api.Models.Repository;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<IAnimalRepository<Animal, int, string>, AnimalManager>();


            var connectionString = builder.Configuration.GetConnectionString("LocalConnection");
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowBlazorClient",
                    policy =>
                    {
                        policy.SetIsOriginAllowed(origin =>
                            new Uri(origin).Host == "localhost")
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
                });
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("AllowBlazorClient");

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
