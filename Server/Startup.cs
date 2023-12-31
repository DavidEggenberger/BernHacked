using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Server.DomainFeatures.ChatAggregate;
using Server.DomainFeatures.CounselingRessourceAggregate;
using Server.Hubs;
using Server.Services;
using Server.Services.AzureSpeech;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Server
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
            services.AddRazorPages();
            services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNameCaseInsensitive = true);
            services.AddSignalR();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddMicrosoftAccount(options =>
                {
                    options.ClientId = Configuration["ClientId"];
                    options.ClientSecret = Configuration["ClientSecret"];
                })
                .AddCookie(options =>
                {
                    options.Cookie.Name = "Auth_Cookie";
                    options.ExpireTimeSpan = TimeSpan.FromDays(1);
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Dargebotene Hand",
                        Version = "v1",
                        Description = "Open API of Dargebotene Hand",
                        Contact = new OpenApiContact
                        {
                            Name = "David Eggenberger"
                        }
                    });
                
            });
            services.Configure<AzureSpeechAnalysisOptions>(options =>
            {
                //options.Endpoint = Configuration["AzureSpeechAnalysisEndpoint"];
                options.APIKey = Configuration["AzureSpeechAnalysisAPIKey"];
            });
            services.AddHttpClient<AzureSpeechAnalysisAPIClient>(options =>
            {
                options.BaseAddress = new Uri("https://switzerlandnorth.api.cognitive.microsoft.com/sts/v1.0/");
            });
            services.AddScoped<AzureSpeechToTextService>();
            services.AddScoped<TextToSpeechService>();

            services.RegisterChatModule();
            services.RegisterCounselingRessourcesModule();

            services.RegisterServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.InjectStylesheet("/swaggerStyles.css");
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotificationHub>("/notificationHub");
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
