using Microsoft.EntityFrameworkCore;
using MailBookApi.Data.Entities;
using System.Linq;
using System;

namespace MailBookApi.Data
{
    public class MailBookDbContext : DbContext
    {
        public DbSet<PackageEntity> Packages { get; internal set; }

        public DbSet<DeliverCompanyEntity> DeliverCompanies { get; set; }

        public MailBookDbContext(DbContextOptions<MailBookDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PackageEntity>().HasKey(p => p.Id);
            modelBuilder.Entity<PackageEntity>().Property(p => p.Id).UseIdentityColumn();
            modelBuilder.Entity<PackageEntity>().HasIndex(p => new { p.DeliverCompanyId, p.PackageNumber }).IsUnique();
            modelBuilder.Entity<PackageEntity>().HasOne(p => p.DeliverCompany).WithMany().HasForeignKey(p => p.DeliverCompanyId).IsRequired();

            modelBuilder.Entity<DeliverCompanyEntity>().Property(p => p.Id).UseIdentityColumn();
            modelBuilder.Entity<DeliverCompanyEntity>().HasIndex(p => p.Name).IsUnique();

            modelBuilder.Entity<DeliverCompanyEntity>().HasData(new DeliverCompanyEntity { Id = 1, Name = "DHL" });
            modelBuilder.Entity<DeliverCompanyEntity>().HasData(new DeliverCompanyEntity { Id = 2, Name = "SCG" });
            modelBuilder.Entity<DeliverCompanyEntity>().HasData(new DeliverCompanyEntity { Id = 3, Name = "Flash" });
            modelBuilder.Entity<DeliverCompanyEntity>().HasData(new DeliverCompanyEntity { Id = 4, Name = "Kerry" });
            modelBuilder.Entity<DeliverCompanyEntity>().HasData(new DeliverCompanyEntity { Id = 5, Name = "Lex" });
            modelBuilder.Entity<DeliverCompanyEntity>().HasData(new DeliverCompanyEntity { Id = 6, Name = "Shoppee" });
            modelBuilder.Entity<DeliverCompanyEntity>().HasData(new DeliverCompanyEntity { Id = 7, Name = "Lazada" });
            modelBuilder.Entity<DeliverCompanyEntity>().HasData(new DeliverCompanyEntity { Id = 8, Name = "Thai Post" });

            modelBuilder.Entity<PackageEntity>().HasData(new PackageEntity { Id = 1, PackageNumber = "PKG000001", DeliverCompanyId = 2, ArrivedDate = new DateTime(2021, 10, 15, 12, 0, 1) });
        }
    }
}