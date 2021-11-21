using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailBookApi.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MailBookApi.Data.Entities;
using MailBookApi.Models;
using MailBookApi.Repos;
using Serilog;
using MailBookApi.AutoMappers;
using MailBookApi.AutoMappers.Resolves;
using MailBookApi.Extensions.Authorize;
using AutoMapper;
using System.Security.Cryptography;
using System.Text;
using MailBookApi.Configure;
using Microsoft.AspNetCore.Authorization;

namespace MailBookApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            WebHostEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MailBookDbContext>(optionBuilder =>
            {
                if (WebHostEnvironment.IsDevelopment())
                {
                    optionBuilder.UseInMemoryDatabase("mailbookapi");
                }
                else
                {
                    var connectionString = Configuration.GetSection("ConnectionStrings:Default").Value;
                    Log.Logger.Information("Use Connection String : {0}", connectionString);
                    optionBuilder.UseSqlServer(connectionString);
                }
            });

            services.AddTransient<EncryptResolver>();
            services.AddTransient<IdEncryptResolver>();
            services.AddTransient<DecryptResolver>();
            services.AddTransient<DeliverCompanyIdDecryptResolver>();

            services.AddSingleton<SecurityKeyConfigureOption>((sp) =>
            {
                var securityKeyConfigureOption = new SecurityKeyConfigureOption();
                this.Configuration.GetSection("Security").Bind(securityKeyConfigureOption);

                if (string.IsNullOrEmpty(securityKeyConfigureOption.Key))
                {
                    throw new Exception("appsetting.xml missing Security:Key value");
                }

                if (string.IsNullOrEmpty(securityKeyConfigureOption.IV))
                {
                    throw new Exception("appsetting.json Security:IV value");
                }

                return securityKeyConfigureOption;
            });

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<DefaultMappingProfile>();
            });

            services.AddScoped<IMailBookRepository, MailBookRepository>();

            services.AddSingleton<IAuthorizationHandler, DefaultAuthorizationHandler>();

            services.AddAuthorization(authorizeConfig =>
            {
                authorizeConfig.AddPolicy("Default", authorizePolicyBuilder =>
                {
                    authorizePolicyBuilder.AddRequirements(new DefaultAuthorizationRequirement());                    
                });
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(jwtBearerOptions =>
                {
                    var issuerKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("vkiydKN8986"));
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = issuerKey,
                        ValidateActor = false,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MailBookApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MailBookApi v1"));

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var mailbookDbContext = (MailBookDbContext)serviceScope.ServiceProvider.GetService<MailBookDbContext>();
                mailbookDbContext.Database.EnsureCreated();
            }

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
