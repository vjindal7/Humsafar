using Domain.Services;

using GoogleGenAI;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using NLog;

using OpenAI;

using System.Text.Json.Serialization;

using WebApi.Middleware;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var logger = LogManager.Setup().LoadConfigurationFromFile("nlog.config", optional: false).GetCurrentClassLogger();

            // Cross Origin Resource Sharing
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    var orgins = Configuration["Settings:AllowedOrigins"].Split(",");
                    policy.WithOrigins(orgins)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .SetIsOriginAllowed(origin => true)
                            .AllowCredentials();
                });
            });

            services.AddMediatR(cf => cf.RegisterServicesFromAssembly(typeof(Mediators.Travel.Ask).Assembly));
            services.AddControllers();

            // Add Swagger services
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            });

            services.AddAuthorization(options =>
            {

            });

            services.AddMvc().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

            // Services
            services.AddTransient<IGenAIClient, GenAIClient>();

            services.AddTransient<ITravelAssistantService, TravelAssistantService>();
            services.AddHttpClient<IAccuWeatherService, AccuWeatherService>();
            services.AddHttpClient<ITrafficService, TrafficService>();
            services.AddTransient<IRouteService, RouteService>();
            services.AddTransient<IAttractionService, AttractionService>();
            services.AddTransient<ITravelScoreService, TravelScoreService>();
            services.AddTransient<IRecommendationService, RecommendationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation("Configuring Web Host Environment");

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI();

            //app.UseHttpsRedirection();

            app.UseStaticFiles(new StaticFileOptions() { ServeUnknownFileTypes = true });

            // order does matter            
            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
