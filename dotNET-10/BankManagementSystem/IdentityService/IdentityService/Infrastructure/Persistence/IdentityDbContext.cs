using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Events;

namespace IdentityService.Infrastructure.Persistence;

public sealed class IdentityDbContext : IdentityDbContext<AppUser, AppRole, string>
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
    {
    }

    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<DomainEventLog> DomainEventLogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(w =>
            w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure AppUser
        builder.Entity<AppUser>(b =>
        {
            b.Property(u => u.FirstName).HasMaxLength(100).IsRequired();
            b.Property(u => u.LastName).HasMaxLength(100).IsRequired();
            b.Property(u => u.CreatedAt).IsRequired().HasDefaultValueSql("GETUTCDATE()");
            b.Property(u => u.IsActive).HasDefaultValue(true);

            b.HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        });

        // Configure AppRole
        builder.Entity<AppRole>(b =>
        {
            b.Property(r => r.Name).HasMaxLength(256).IsRequired();
            b.Property(r => r.NormalizedName).HasMaxLength(256).IsRequired();
            b.Property(r => r.CreatedAt).IsRequired().HasDefaultValueSql("GETUTCDATE()");

            b.HasMany(r => r.UserRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
        });

        // Configure AppUserRole
        builder.Entity<AppUserRole>(b =>
        {
            b.HasKey(ur => new { ur.UserId, ur.RoleId });
            b.Property(ur => ur.AssignedAt).IsRequired().HasDefaultValueSql("GETUTCDATE()");
        });

        // Configure RefreshToken
        builder.Entity<RefreshToken>(b =>
        {
            b.HasKey(rt => rt.Id);
            b.Property(rt => rt.Id).HasDefaultValueSql("NEWID()");
            b.Property(rt => rt.UserId).IsRequired();
            b.Property(rt => rt.Token).IsRequired();
            b.Property(rt => rt.ExpiresAt).IsRequired();
            b.Property(rt => rt.IssuedAt).IsRequired().HasDefaultValueSql("GETUTCDATE()");

            b.HasOne(rt => rt.User)
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .IsRequired();

            b.HasIndex(rt => rt.Token).IsUnique();
            b.HasIndex(rt => rt.UserId);
        });

        // Configure DomainEventLog
        builder.Entity<DomainEventLog>(b =>
        {
            b.HasKey(el => el.Id);
            b.Property(el => el.Id).HasDefaultValueSql("NEWID()");
            b.Property(el => el.EventType).IsRequired();
            b.Property(el => el.EventData).IsRequired();
            b.Property(el => el.CreatedAt).IsRequired().HasDefaultValueSql("GETUTCDATE()");
            b.Property(el => el.AggregateId).IsRequired();
        });

        SeedRoles(builder);
    }

    private static void SeedRoles(ModelBuilder builder)
    {
        builder.Entity<AppRole>().HasData(
            new AppRole
            {
                Id = "1",
                Name = "Admin",
                NormalizedName = "ADMIN",
                Description = "Administrator role with full system access",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new AppRole
            {
                Id = "2",
                Name = "User",
                NormalizedName = "USER",
                Description = "Standard user role",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new AppRole
            {
                Id = "3",
                Name = "Manager",
                NormalizedName = "MANAGER",
                Description = "Manager role with supervisory access",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
