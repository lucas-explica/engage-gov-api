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
    public DbSet<Vote> Votes => Set<Vote>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Citizen entity
        modelBuilder.Entity<Citizen>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.DocumentNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.IsEmailVerified).IsRequired();
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();

            // Indexes for performance
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.DocumentNumber).IsUnique();

            // Relationships
            entity.HasMany(e => e.Proposals)
                .WithOne(p => p.Citizen)
                .HasForeignKey(p => p.CitizenId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Votes)
                .WithOne(v => v.Citizen)
                .HasForeignKey(v => v.CitizenId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Comments)
                .WithOne(c => c.Citizen)
                .HasForeignKey(c => c.CitizenId)
                .OnDelete(DeleteBehavior.Restrict);
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
            entity.Property(e => e.VoteCount).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();

            // Indexes for performance
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.Type);
            entity.HasIndex(e => e.CitizenId);
            entity.HasIndex(e => e.CreatedAt);

            // Relationships
            entity.HasMany(e => e.Votes)
                .WithOne(v => v.Proposal)
                .HasForeignKey(v => v.ProposalId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Comments)
                .WithOne(c => c.Proposal)
                .HasForeignKey(c => c.ProposalId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Vote entity
        modelBuilder.Entity<Vote>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.IsUpvote).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();

            // Composite index to prevent duplicate votes
            entity.HasIndex(e => new { e.ProposalId, e.CitizenId }).IsUnique();
        });

        // Configure Comment entity
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Content).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.CreatedAt).IsRequired();

            // Indexes
            entity.HasIndex(e => e.ProposalId);
            entity.HasIndex(e => e.CitizenId);
        });
    }
}
