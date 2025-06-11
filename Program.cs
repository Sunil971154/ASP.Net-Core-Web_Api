
using JerEntryWebApp.Data;
using JrEntryWebApi.Services;
using Microsoft.EntityFrameworkCore;

namespace JrEntryWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.


            builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("Conn")));// yaha bus register hua hai , connection string app setting me bana lo , 

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IJEService, JournalEntryService>();

            builder.Services.AddControllers();
           
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            Console.WriteLine(" Program Stared Now ");


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
