using System;
using System.Linq;
using Logic.Models;
using Microsoft.EntityFrameworkCore;

namespace Logic
{
    public class Database : DbContext
    {
        public Database(DbContextOptions<Database> options)
            : base(options)
        {
            SampleData.Initialize(this);
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductGroup>()
                    .HasMany(rec => rec.Suppliers)
                    .WithMany(rec => rec.ProductGroups)
                    .UsingEntity(rec => rec.ToTable("SupplierProducts"));
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Consignment> Consignments { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductGroup> ProductGroups { get; set; }

        public DbSet<Stock> Stocks { get; set; }

        public DbSet<TableProduct> TableProducts { get; set; }

        public DbSet<Division> Divisions { get; set; }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }
    }
}
