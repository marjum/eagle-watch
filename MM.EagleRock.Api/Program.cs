using MM.EagleRock.Business;
using MM.EagleRock.Business.Devices;
using MM.EagleRock.Contract;
using MM.EagleRock.Contract.Cache;
using MM.EagleRock.Contract.RoadTraffic;
using MM.EagleRock.DAL.Cache;
using System.Text.Json.Serialization;

namespace MM.EagleRock.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            configureServiceDependencies(builder.Services, builder.Configuration);

            builder.Services
                .AddControllers()
                .AddJsonOptions(options => 
                    { 
                        // Prefer using string enum values to improve API consumer-friendliness
                        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static void configureServiceDependencies(IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options => 
                {
                    options.Configuration = 
                        $"{configuration.GetValue<string>("RedisCache:Host")}:{configuration.GetValue<int>("RedisCache:Port")}";
                });
            
            // Singletons
            services.AddSingleton<ICacheService, RedisCacheService>();
            services.AddSingleton<IDeviceRegistrar, FakeDeviceRegistrar>();

            // Scoped per request
            services.AddScoped<IDeviceSummaryCache, DeviceSummaryCache>();
            services.AddScoped<IRoadTrafficOfficer, RoadTrafficOfficer>();
        }
    }
}