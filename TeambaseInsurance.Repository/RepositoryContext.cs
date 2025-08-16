using Microsoft.EntityFrameworkCore;
using Entities.Models;
using Entities.Contracts;

namespace TeambaseInsurance.Repository
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {
        }

        // DbSets for entities
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureEmployee(modelBuilder);

            // Configure common properties for time-stamped models
            ConfigureTimeStampedModels(modelBuilder);
        }

        private void ConfigureEmployee(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.DateOfBirth)
                    .IsRequired();

                entity.Property(e => e.JoinDate)
                    .IsRequired();

                entity.Property(e => e.PolicyEndDate)
                    .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.LastModified)
                    .IsRequired(false);

                // Add indexes for better performance
                entity.HasIndex(e => e.LastName);
                entity.HasIndex(e => e.JoinDate);
                entity.HasIndex(e => e.PolicyEndDate);
            });
        }

        private void ConfigureTimeStampedModels(ModelBuilder modelBuilder)
        {
            // Configure common properties for all entities implementing ITimeStampedModel
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ITimeStampedModel).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property("CreatedAt")
                        .IsRequired()
                        .HasDefaultValueSql("GETUTCDATE()");

                    modelBuilder.Entity(entityType.ClrType)
                        .Property("LastModified")
                        .IsRequired(false);
                }
            }
        }

        public override int SaveChanges()
        {
            UpdateTimeStamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimeStamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimeStamps()
        {
            var entries = ChangeTracker.Entries<ITimeStampedModel>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.LastModified = DateTime.UtcNow;
                }
            }
        }
    }
}
