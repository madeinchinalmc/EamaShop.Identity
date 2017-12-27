using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;

namespace EamaShop.Identity.API
{
    /// <summary>
    /// app start
    /// </summary>
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }
        /// <summary>
        /// config file 
        /// </summary>
        public IConfiguration Configuration { get; }
        /// <summary>
        /// hoting env.
        /// </summary>
        public IHostingEnvironment Environment { get; }
        /// <summary>
        /// Configure Service Container
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // for restful api;
            services.AddMvcCore(MvcConfig.Configure)
                .AddApiExplorer()
                .AddDataAnnotations()
                .AddJsonFormatters(opt => opt.ContractResolver = null)
                .AddFormatterMappings()
                .AddCors(opt => opt.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin()
                                                                              .AllowAnyMethod()
                                                                              .AllowCredentials()
                                                                              .AllowAnyHeader()))
                .AddAuthorization();

            // for authentication 
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    var parameters = new TokenValidationParameters()
                    {
                        NameClaimType = ClaimTypes.Name,
                        RoleClaimType = ClaimTypes.Role,
                        ValidIssuer = ClaimsIdentity.DefaultIssuer,
                        ValidAudience = EamaDefaults.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(EamaDefaults.JwtBearerSignKey)),
                        TokenDecryptionKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(EamaDefaults.JwtBearerTokenKey))
                    };

                    options.TokenValidationParameters = parameters;
                });

            services.AddResponseCaching();


            //services.AddRabbitMQEventBus(opt =>
            //{
            //    opt.ConnectRetryCount = configuration.GetValue<int>("RabbitMQConnectionRetry");
            //    opt.Host = configuration.GetValue<string>("RabbitMQHost");
            //    opt.PublishRetryCount = configuration.GetValue<int>("EventBusPublishRetryCount");
            //    opt.UserName = configuration.GetValue<string>("RabbitMQUserName");
            //    opt.Password = configuration.GetValue<string>("RabbitMQPassword");
            //});

            //services.AddDistributedRedisLock(opt =>
            //{
            //    opt.Configuration = configuration["RedisLockInstanceName"];
            //    opt.InstanceName = configuration["RedisInstanceName"];
            //});

            services.AddDistributedRedisCache(opt =>
            {
                opt.Configuration = Configuration["RedisConnectionConfiguration"];
                opt.InstanceName = Configuration["RedisInstanceName"];
            });
            if (Environment.IsDevelopment())
            {
                services.AddSwaggerGen(opt =>
                {
                    var apiInfo = new Info()
                    {
                        Title = "Microservice of Identity",
                        Version = "v1",
                        Description = $"The HTTP API Microservice of Identity",
                        TermsOfService = "Terms Of Service"
                    };
                    opt.SwaggerDoc("v1", apiInfo);

                    opt.IgnoreObsoleteActions();
                    var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.xml", SearchOption.AllDirectories);
                    foreach (var f in files)
                    {
                        opt.IncludeXmlComments(f);
                    }
                    opt.OperationFilter<AuthorizeCheckOperationFilter>();
                });
            }
            services.AddIdentityServices(Configuration.GetConnectionString("Master"));
        }


        /// <summary>
        /// configure middle ware
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="logger"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory logger)
        {
            app.UseResponseCaching();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();

            if (env.IsDevelopment())
            {
                app.UseSwagger()
                .UseSwaggerUI(SwaggerConfig.Configure);

                app.UseDeveloperExceptionPage();
            }
            LogFactoryConfig.Configure(logger, LogLevel.Debug);

            try
            {
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    scope.ServiceProvider
                        .GetRequiredService<DbContext>()
                        .Database.EnsureCreated();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
