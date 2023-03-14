using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Discount.Grpc.Protos;

namespace Basket.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

            // Redis Configuration
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = config.GetValue<string>("CacheSettings:ConnectionString");
            });

            // General Configuration
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();

            // Grpc Configuration
            builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>
                (o => o.Address = new Uri(config["GrpcSettings:DiscountUrl"]));

            builder.Services.AddScoped<DiscountGrpcService>();

            // Add services to the container.

            builder.Services.AddControllers();
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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}