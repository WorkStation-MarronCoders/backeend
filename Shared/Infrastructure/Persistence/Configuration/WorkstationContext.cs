using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using workstation_backend.OfficesContext.Domain.Models.Entities;
using workstation_backend.UserContext.Domain.Models.Entities;
namespace workstation_backend.Shared.Infrastructure.Persistence.Configuration;

public class WorkstationContext(DbContextOptions options) : DbContext(options)
{
    //public DbSet<Entidad> Entidad {get; set;}
    public DbSet<Office> Offices { get; set; }
    public DbSet<OfficeService> Services { set; get; }
    public DbSet<Rating> Ratings { get; set; }

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

            entity.Property(c => c.Capacity).IsRequired(); 
            entity.Property(c => c.CostPerDay).IsRequired();
            entity.Property(c => c.Available).IsRequired();

            entity.HasMany(o => o.Services)
                .WithOne(s => s.Office)
                .HasForeignKey(s => s.OfficeId)
                .OnDelete(DeleteBehavior.Cascade); 


            entity.HasMany(o => o.Ratings)
                .WithOne(r => r.Office)
                .HasForeignKey(r => r.OfficeId)
                .OnDelete(DeleteBehavior.Cascade); 
        });
        // OfficeService Entity Configuration
        builder.Entity<OfficeService>(entity =>{
            entity.ToTable("OfficeServices");
            entity.HasKey(c => c.Id);

            entity.HasIndex(s => new { s.OfficeId, s.Name }).IsUnique();

            entity.Property(c => c.Name).IsRequired().HasMaxLength(200);
            entity.Property(c => c.Description).IsRequired().HasMaxLength(500);
            entity.Property(c => c.Cost).IsRequired();
        });
        
        builder.Entity<Rating>(entity =>
        {
            entity.ToTable("Ratings");
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Score).IsRequired();
            entity.Property(r => r.Comment).HasMaxLength(500);
            
        });
        
        builder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(u => u.Id);

            entity.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(u => u.LastName).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Dni).IsRequired().HasMaxLength(20);
            entity.HasIndex(u => u.Dni).IsUnique();

            entity.Property(u => u.PhoneNumber).HasMaxLength(20);
            entity.Property(u => u.Email).HasMaxLength(100);

            entity.Property(u => u.Role)
                .IsRequired()
                .HasConversion<int>(); 

            entity.Property(u => u.CreatedAt).IsRequired();
        });


    }
}
