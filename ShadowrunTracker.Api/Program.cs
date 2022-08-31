namespace ShadowrunTracker.Api
{
    using ShadowrunTracker.Api.Hubs;
    using ShadowrunTracker.Api.TimerFeatures;
    using ShadowrunTracker.Client;

    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await WebApplication
                .CreateBuilder(args)
                .RegisterServices()
                .Build()
                .ConfigureServices()
                .RunAsync();
        }

        private static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
        {
            // Add services to the container.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder
                    .WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSignalR()
                .AddJsonProtocol(o =>
                {
                    o.PayloadSerializerOptions.Converters.Add(new RecordJsonConverter());
                });

            builder.Services.AddSingleton<TimerManager>();

            builder.Services.AddMemoryCache();

            return builder;
        }

        private static WebApplication ConfigureServices(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.MapControllers();
            app.MapHub<ChartHub>("hubs/chart");
            app.MapHub<EncounterHub>("hubs/encounter");

            return app;
        }
    }
}