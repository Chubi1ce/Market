
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Market.Abstrctions;
using Market.Models;
using Market.Repo;
using Microsoft.Extensions.FileProviders;

namespace Market
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

            builder.Services.AddAutoMapper(typeof(MappingProfile));
            
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            
            var config = new ConfigurationBuilder();
            config.AddJsonFile("appsettings.json");
            var cfg = config.Build();
            
            builder.Host.ConfigureContainer<ContainerBuilder>(cb =>
            {
                cb.Register(c => new MarketContext(cfg.GetConnectionString("DB"))).InstancePerDependency();

                cb.RegisterType<ProductRepository>().As<IProductRepository>();
                cb.RegisterType<ProductGroupRepository>().As<IProductGroupRepository>();
            });

            builder.Services.AddMemoryCache(options => options.TrackStatistics = true);

            //так должно быть по хорошему
            //builder.Services.AddSingleton<IProducrRepository, ProducrRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            var staticFilesPath = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles");
            Directory.CreateDirectory(staticFilesPath);

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(staticFilesPath),
                RequestPath = "/static"
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
