using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using ProductStorageAPI.DTO;
using ProductStorageAPI.Repository;
using SharedModels.Models;

namespace ProductStorageAPI
{
    public class Query
    {
        public List<StorageDto> GetStorages([Service] ProductStorageRepository repo) => repo.GetStorages().ToList();
    }
    public class Mutation
    {
        public StoragePayload AddStorage(StorageInput input, [Service] ProductStorageRepository repo)
        {
            string name = input.name;
            int curId = repo.GetStorageId(name);
            if (curId == -1)
            {
                curId = repo.AddStorage(name, input.description);
                return new StoragePayload(new StorageDto()
                {
                    ID = curId,
                    Name = name,
                    Description = input.description
                });
            }
            else throw new Exception($"Склад с названием \"{name}\" уже существует (ID={curId}).");
        }
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var graphQl = builder.Services.AddGraphQLServer();
            graphQl.AddQueryType<Query>();
            graphQl.AddMutationType<Mutation>();


            builder.Services.AddSingleton<ProductStorageRepository>();

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            var config = new ConfigurationBuilder();
            config.AddJsonFile("appsettings.json");
            var configRoot = config.Build();

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
            {
                containerBuilder.Register(c => new ProductContext(configRoot.GetConnectionString("db"))).InstancePerDependency();
            });



            var app = builder.Build();

            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();

            app.MapControllers();

            app.MapGraphQL();

            app.Run();
        }
    }
}
