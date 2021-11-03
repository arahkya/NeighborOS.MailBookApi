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
using MailBookApi.Data.Entities;
using MailBookApi.Models;
using MailBookApi.Repos;
using Serilog;
using MailBookApi.AutoMappers;
using MailBookApi.AutoMappers.Resolves;
using AutoMapper;
using System.Security.Cryptography;
using System.Text;
using MailBookApi.Configure;

namespace MailBookApi
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
            services.AddDbContext<MailBookDbContext>(optionBuilder =>
                        {
                            var connectionString = Configuration.GetSection("ConnectionStrings:Default").Value;

                            Log.Logger.Information("Use Connection String : {0}", connectionString);

                            optionBuilder.UseSqlServer(connectionString);
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
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MailBookApi v1"));
            }

            // using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            // {
            //     var mailbookDbContext = (MailBookDbContext)serviceScope.ServiceProvider.GetService<MailBookDbContext>();
            //     mailbookDbContext.Database.EnsureCreated();
            // }

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
