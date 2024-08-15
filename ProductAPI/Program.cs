
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CSharpWebApplication.Models;
using Microsoft.Extensions.FileProviders;

namespace CSharpWebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddMemoryCache(options =>
            {
                options.TrackStatistics = true;
            });

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            
            var config = new ConfigurationBuilder();
            config.AddJsonFile("appsettings.json");
            var configRoot = config.Build();

            builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
            {
                containerBuilder.Register(c => new ProductContext(configRoot.GetConnectionString("db"))).InstancePerDependency();
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            var staticFilePath = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles");
            Directory.CreateDirectory(staticFilePath);


            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(staticFilePath),
                RequestPath = "/static"
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
