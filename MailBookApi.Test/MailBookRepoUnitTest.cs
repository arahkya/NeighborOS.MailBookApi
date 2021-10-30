using Xunit;
using MailBookApi.Repos;
using Microsoft.Extensions.DependencyInjection;
using MailBookApi.Data;
using Microsoft.EntityFrameworkCore;
using MailBookApi.Models;
using System;
using System.Linq;
using MailBookApi.AutoMappers;
using MailBookApi.AutoMappers.Resolves;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MailBookApi.Configure;

namespace MailBookApi.Test
{
    public class TestStartUp : IDisposable
    {
        private IServiceScope _Scope;

        public IMailBookRepository Repository { get; private set; }

        public MailBookDbContext DbContext { get; private set; }

        public TestStartUp()
        {
            var services = new ServiceCollection();
            services.AddDbContext<MailBookDbContext>(opt =>
            {
                opt.UseInMemoryDatabase("MailBookDb");
            });

            services.AddTransient<EncryptResolver>();
            services.AddTransient<IdEncryptResolver>();
            services.AddTransient<DecryptResolver>();
            services.AddTransient<DeliverCompanyIdDecryptResolver>();

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<DefaultMappingProfile>();
            });

            services.AddScoped<IMailBookRepository, MailBookRepository>();

            services.AddSingleton<SecurityKeyConfigureOption>((sp) =>
            {
                var securityKeyConfigureOption = new SecurityKeyConfigureOption
                {
                    Key = "00000898609177123283100601614660",
                    IV = "000arahkyambupah"
                };

                return securityKeyConfigureOption;
            });

            var servicesProvider = services.BuildServiceProvider();
            _Scope = servicesProvider.CreateScope();
            var scopeServiceProvider = _Scope.ServiceProvider;

            var dbContext = (MailBookDbContext)scopeServiceProvider.GetService(typeof(MailBookDbContext));
            dbContext.Database.EnsureCreated();

            DbContext = dbContext;

            Repository = (IMailBookRepository)scopeServiceProvider.GetService(typeof(IMailBookRepository));
        }

        public void Dispose()
        {
            _Scope.Dispose();
        }
    }

    public class MailBookRepoUnitTest : IClassFixture<TestStartUp>
    {
        private TestStartUp _startup;

        public MailBookRepoUnitTest(TestStartUp startup)
        {
            this._startup = startup;
        }

        [Fact]
        public void TestGetDeliverCompany()
        {
            // Arrange            
            var mailBookRepo = _startup.Repository;

            // Action
            var list = mailBookRepo.ListDeliverCompanyAsync();

            // Assert
            Assert.NotEmpty(list.Result);
        }

        [Fact]
        public void TestCreatePackage()
        {
            // Arrange
            var mailBookRepo = _startup.Repository;
            var deliverCompanyId = mailBookRepo.ListDeliverCompanyAsync().Result.First().Id;
            var model = new CreatePackageModel
            {
                PackageNumber = "PKG00001",
                ArrivedDate = new DateTime(2021, 10, 15, 12, 0, 0),
                DeliverCompanyId = deliverCompanyId
            };

            // Action
            mailBookRepo.CreatePackageEntryAsync(model);

            // Assert
            var packageEntity = _startup.DbContext.Packages.SingleOrDefaultAsync(p => p.PackageNumber == "PKG00001").Result;
            Assert.NotNull(packageEntity);
        }

        [Fact]
        public void TestListPackage()
        {
            // Arrange            
            var mailBookRepo = _startup.Repository;

            // Action
            var list = mailBookRepo.ListPackageAsync();

            // Assert
            Assert.NotEmpty(list.Result);
        }

    }
}
