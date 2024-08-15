using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.OpenApi.Models;

namespace Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // �������: ��������� �������, ����������� ��� SwaggerForOcelot
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddMvc();
            builder.Services.AddSwaggerGen();

            // �������: ������������� Ocelot
            var config = new ConfigurationBuilder()
                .AddJsonFile("ocelot.json")
                .Build();
            builder.Services.AddOcelot(config);

            // �������: ��������� ������ SwaggerForOcelot
            builder.Services.AddSwaggerForOcelot(config);
            
            var app = builder.Build();

            // �������: ������������ Swagger MiddleWare 
            app.UseSwagger();

            // �������: ������������ Ocelot MiddleWare
            app.UseSwaggerForOcelotUI(opt =>
            {
                opt.PathToSwaggerGenerator = "/swagger/docs";
            });

            app.UseOcelot().Wait();

            //  �������: ������������ Https Redirection Middleware
            app.UseHttpsRedirection();

            app.Run();
        }
    }
}
