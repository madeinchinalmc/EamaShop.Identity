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
using NLog.Config;
using NLog.Targets;
using Microsoft.Extensions.DependencyInjection.Extensions;
using EamaShop.Identity.API.Dto;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

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
            if (!services.Any(x => x.ServiceType == typeof(IHttpContextAccessor)))
            {
                Console.WriteLine("未注册HttpAccessor");
                services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            }
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

                    opt.AddSecurityDefinition("JsonWebTokenBearer", new ApiKeyScheme()
                    {
                        Description = "登陆授权后获得的JsonWebToken 可以在<a href=\"http://120.78.181.207:8989/swagger\" target=\"_blank\">授权登陆页面</a>手动获取",
                        In = "header",
                        Name = "Authorization",
                        Type = "apiKey",
                    });
                });
            }
            services.AddIdentityServices(Configuration.GetConnectionString("Master"));
            ConfigureActionDTOParameter(services);
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

            //app.UseStaticFiles(options: new StaticFileOptions()
            //{
            //    OnPrepareResponse = ctx =>
            //        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=600"),
            //    FileProvider = env.WebRootFileProvider,
            //});
            app.UseCors("CorsPolicy");

            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();

            if (env.IsDevelopment())
            {
                app.UseSwagger()
                .UseSwaggerUI(SwaggerConfig.Configure);

                app.UseDeveloperExceptionPage();
            }

            ConfigureLogger(logger);
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
        private void ConfigureActionDTOParameter(IServiceCollection services)
        {
            services.TryAddScoped<ImagePostDTO>();
        }
        private void ConfigureLogger(ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();
            var config = new LoggingConfiguration();
            var title = "${longdate}|事件Id=${event-properties:item=EventId.Id}|${logger}";
            var body = "${newline}${message}";
            var layout = title + body + "${newline}ErrorMessage: ${exception}${newline}${stacktrace}${newline}####################################################################";

            // 普通日志
            var fileTarge = new FileTarget()
            {
                Layout = layout,
                ArchiveNumbering = ArchiveNumberingMode.Sequence,
                FileName = "../logs/${shortdate}/${level}.log",
                FileNameKind = FilePathKind.Relative,
                ArchiveFileKind = FilePathKind.Relative,
                ArchiveFileName = "../logs/${shortdate}/${level}-{####}.log",
                ArchiveEvery = FileArchivePeriod.None,
                ArchiveAboveSize = 1024 * 1024
            };

            // 微软日志
            var msTarge = new FileTarget()
            {
                Layout = layout,
                ArchiveNumbering = ArchiveNumberingMode.Sequence,
                FileName = "../logs/${shortdate}/Microsoft.log",
                FileNameKind = FilePathKind.Relative,
                ArchiveFileKind = FilePathKind.Relative,
                ArchiveFileName = "../logs/${shortdate}/${level}-{####}.log",
                ArchiveEvery = FileArchivePeriod.None,
                ArchiveAboveSize = 1024 * 1024
            };


            config.AddTarget("file", fileTarge);
            config.AddTarget("microsoft", msTarge);
            config.AddTarget("skip", new NullTarget());

            // 预发布和测试环境允许trace和debug
            if (Environment.IsDevelopment() || Environment.IsStaging())
            {
                config.AddRuleForOneLevel(NLog.LogLevel.Trace, "file");
                config.AddRuleForOneLevel(NLog.LogLevel.Debug, "file");
            }
            // 预发布允许信息级别
            if (Environment.IsStaging())
            {
                config.AddRuleForOneLevel(NLog.LogLevel.Info, "file");
            }
            // 允许应用
            config.AddRule(NLog.LogLevel.Warn, NLog.LogLevel.Off, "file", "EamaShop.*");
            // 微软的debug和trace日志跳过
            config.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Debug, "skip", "Microsoft.*");
            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Off, "microsoft", "Microsoft.*");
            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Off, "microsoft", "EntityFramework*");
            loggerFactory.ConfigureNLog(config);
        }
    }
}
