using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using workstation_backend.OfficesContext.Domain.Models.Entities;
namespace workstation_backend.Shared.Infrastructure.Persistence.Configuration;

public class WorkstationContext(DbContextOptions options) : DbContext(options)
{
    //public DbSet<Entidad> Entidad {get; set;}
    public DbSet<Office> Offices { get; set; }
    public DbSet<OfficeService> Services { set; get; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        base.OnConfiguring(builder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Office Entity Configuration
        builder.Entity<Office>(entity =>
        {
            entity.ToTable("Offices");
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Location).IsRequired().HasMaxLength(200);
            entity.HasIndex(c => c.Location).IsUnique();

            entity.Property(c => c.Capacity).IsRequired().HasMaxLength(4);

            entity.Property(c => c.CostPerDay).IsRequired();

            entity.Property(c => c.Available).IsRequired();

            entity.HasMany(o => o.Services)
                .WithOne(s => s.Office)
                .HasForeignKey(s => s.Id)
                .OnDelete(DeleteBehavior.Cascade);
        });
        // OfficeService Entity Configuration
        builder.Entity<OfficeService>(entity =>
        {
            entity.ToTable("OfficeServices");
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Name).IsRequired().HasMaxLength(200);
            entity.HasIndex(c => c.Name).IsUnique();

            entity.Property(c => c.Description).IsRequired().HasMaxLength(500);
            entity.Property(c => c.Cost).IsRequired();
        });

        }
}
