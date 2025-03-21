﻿//using System;
//using System.Collections.Generic;
//using Microsoft.EntityFrameworkCore;
//using StealAllTheCats.Models;

//namespace StealAllTheCats.Data;

//public partial class StealAllTheCatsDbContext : DbContext
//{
//    public StealAllTheCatsDbContext()
//    {
//    }

//    public StealAllTheCatsDbContext(DbContextOptions<StealAllTheCatsDbContext> options)
//        : base(options)
//    {
//    }

//    public virtual DbSet<Cat> Cats { get; set; }

//    public virtual DbSet<Tag> Tags { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//    {
//        if (!optionsBuilder.IsConfigured) // Prevent double configuration
//        {
//            // Check if we are in a test environment
//            var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

//            if (environment == "Testing")
//            {
//                optionsBuilder.UseInMemoryDatabase("TestDatabase"); //Use InMemoryDatabase for tests
//            }
//            else
//            {
//                optionsBuilder.UseSqlServer("Name=DefaultConnection"); //Use SQL Server for production
//            }
//        }
//    }

//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        modelBuilder.Entity<Cat>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__Cats__3214EC0767B9E8CC");

//            entity.HasIndex(e => e.CatId, "UQ__Cats__6A1C8AFBC43B071F").IsUnique();

//            entity.Property(e => e.CatId).HasMaxLength(50);
//            entity.Property(e => e.Created)
//                .HasDefaultValueSql("(getdate())")
//                .HasColumnType("datetime");

//            entity.HasMany(d => d.Tags).WithMany(p => p.Cats)
//                .UsingEntity<Dictionary<string, object>>(
//                    "CatTag",
//                    r => r.HasOne<Tag>().WithMany()
//                        .HasForeignKey("TagId")
//                        .HasConstraintName("FK__CatTags__TagId__403A8C7D"),
//                    l => l.HasOne<Cat>().WithMany()
//                        .HasForeignKey("CatId")
//                        .HasConstraintName("FK__CatTags__CatId__3F466844"),
//                    j =>
//                    {
//                        j.HasKey("CatId", "TagId").HasName("PK__CatTags__BC4B4560BCCCEF6D");
//                        j.ToTable("CatTags");
//                    });
//        });

//        modelBuilder.Entity<Tag>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__Tags__3214EC071EA60E88");

//            entity.HasIndex(e => e.Name, "UQ__Tags__737584F63007F50F").IsUnique();

//            entity.Property(e => e.Created)
//                .HasDefaultValueSql("(getdate())")
//                .HasColumnType("datetime");
//            entity.Property(e => e.Name).HasMaxLength(150);
//        });

//        OnModelCreatingPartial(modelBuilder);
//    }

//    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
//}
// Data/StealAllTheCatsDbContext.cs
using Microsoft.EntityFrameworkCore;
using StealAllTheCats.Models;

namespace StealAllTheCats.Data;
public class StealAllTheCatsDbContext : DbContext
{
    public StealAllTheCatsDbContext(DbContextOptions<StealAllTheCatsDbContext> options)
        : base(options) { }

    public DbSet<Cat> Cats { get; set; }
    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cat>()
            .HasIndex(c => c.CatId)
            .IsUnique();
        //define a many-to-many relationship between Cat and Tag tables
        modelBuilder.Entity<Cat>()
            .HasMany(c => c.Tags)
            .WithMany(t => t.Cats)
            .UsingEntity(j => j.ToTable("CatTags"));

        modelBuilder.Entity<Tag>()
            .HasIndex(t => t.Name)
            .IsUnique();
    }
}
