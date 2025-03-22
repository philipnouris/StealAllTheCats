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