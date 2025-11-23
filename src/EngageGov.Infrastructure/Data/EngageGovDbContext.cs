using EngageGov.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EngageGov.Infrastructure.Data;

/// <summary>
/// Entity Framework DbContext for the application
/// Implements data access layer
/// </summary>
public class EngageGovDbContext : DbContext
{
    public EngageGovDbContext(DbContextOptions<EngageGovDbContext> options)
        : base(options)
    {
    }

    public DbSet<Citizen> Citizens => Set<Citizen>();
    public DbSet<Proposal> Proposals => Set<Proposal>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Citizen entity
        modelBuilder.Entity<Citizen>(entity =>
        {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired(false).HasMaxLength(200);
                entity.Property(e => e.Email).IsRequired(false).HasMaxLength(255);
                entity.Property(e => e.Phone).IsRequired(false).HasMaxLength(20);
                entity.Property(e => e.Neighborhood).IsRequired(false).HasMaxLength(100);
                entity.Property(e => e.Points).IsRequired().HasDefaultValue(0);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired(false);

                // Índices para performance
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Phone);
                // Nenhum relacionamento obrigatório
        });

        // Configure Proposal entity
        modelBuilder.Entity<Proposal>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(5000);
            entity.Property(e => e.Location).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Type).IsRequired();
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.Priority).IsRequired();
            entity.Property(e => e.EstimatedCost).HasColumnType("decimal(18,2)");
            entity.Property(e => e.CreatedAt).IsRequired();

            // Indexes for performance
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.Type);
            entity.HasIndex(e => e.CitizenId);
            entity.HasIndex(e => e.CreatedAt);
            // Nenhum relacionamento obrigatório
        });

    // Removido: sistema de votos

        // Configure Comment entity
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Content).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.CreatedAt).IsRequired();

            // Indexes
            entity.HasIndex(e => e.ProposalId);
            entity.HasIndex(e => e.CitizenId);
            // Nenhum relacionamento obrigatório
        });
    }
}
