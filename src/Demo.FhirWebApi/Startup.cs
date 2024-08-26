using Demo.Core.Interfaces;
using Demo.Core.Services;
using Demo.Core.Settings;
using Microsoft.OpenApi.Models;
using Demo.FhirWebApi.Middleware;
using Demo.FhirWebApi.MediaFormatters;

namespace Demo.FhirWebApi
{
    public class Startup
    {
        private const string Name = "Demo API";
        private const string Version = "v2";

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // CORS configuration
            services.AddCors(options => options.AddPolicy("AllowAny", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));

            // OpenAPI
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc(Version, new OpenApiInfo
                {
                    Title = Name,
                    Version = Version
                });
            });

            // FHIR formatters
            services.AddControllers(options =>
            {
                options.InputFormatters.Insert(0, new JsonFhirInputFormatter());
                options.OutputFormatters.Insert(0, new JsonOutputFormatter());
            });

            // Add DB context

            services.AddHttpContextAccessor();

            // Dependency Injection
            services.AddScoped<IFhirService, FhirService>();
            services.AddScoped<IDocumentService, DocumentService>();
            
            services.AddOptions<AppSettings>();
            services.Configure<AppSettings>(Configuration.GetSection("Demo"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<FhirExceptionMiddleware>();

            // Enable Swagger
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"swagger/{Version}/swagger.json", $"{Name} {Version}");
                c.RoutePrefix = string.Empty;
                c.DocumentTitle = Name;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowAny");

            app.UseAuthorization();

            // Authentication - to use certificates
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}