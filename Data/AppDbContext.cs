using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Moeda> Moedas { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Moeda>(entity =>
        {
            entity.ToTable("Moedas");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Simbolo).IsRequired().HasMaxLength(10);
            entity.Property(e => e.Preco).HasColumnType("decimal(18,8)");
            entity.HasIndex(e => e.Simbolo).IsUnique();
        });
    }
}