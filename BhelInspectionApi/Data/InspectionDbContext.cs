using Microsoft.EntityFrameworkCore;
using BhelInspectionApi.Models.Entities;

namespace BhelInspectionApi.Data
{
    public class InspectionDbContext : DbContext
    {
        public InspectionDbContext(DbContextOptions<InspectionDbContext> options) : base(options)
        {
        }

        public DbSet<InspectionForm> InspectionForms { get; set; }
        public DbSet<BladeMeasurement> BladeMeasurements { get; set; }
        public DbSet<InspectionAnalysis> InspectionAnalyses { get; set; }
        public DbSet<MeasurementDeviation> MeasurementDeviations { get; set; }
        public DbSet<ToleranceSpecification> ToleranceSpecifications { get; set; }
        public DbSet<InspectionAuditLog> InspectionAuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure InspectionForm
            modelBuilder.Entity<InspectionForm>(entity =>
            {
                entity.HasIndex(e => e.JobNumber).HasDatabaseName("IX_InspectionForm_JobNumber");
                entity.HasIndex(e => e.FormId).HasDatabaseName("IX_InspectionForm_FormId");
                entity.HasIndex(e => e.Status).HasDatabaseName("IX_InspectionForm_Status");
                entity.HasIndex(e => e.InspectionDate).HasDatabaseName("IX_InspectionForm_InspectionDate");
                entity.HasIndex(e => new { e.Customer, e.PartNumber }).HasDatabaseName("IX_InspectionForm_Customer_PartNumber");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            // Configure BladeMeasurement
            modelBuilder.Entity<BladeMeasurement>(entity =>
            {
                entity.HasIndex(e => e.InspectionFormId).HasDatabaseName("IX_BladeMeasurement_InspectionFormId");
                entity.HasIndex(e => e.BladeNumber).HasDatabaseName("IX_BladeMeasurement_BladeNumber");
                entity.HasIndex(e => new { e.InspectionFormId, e.BladeNumber })
                      .IsUnique()
                      .HasDatabaseName("IX_BladeMeasurement_InspectionFormId_BladeNumber");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(d => d.InspectionForm)
                      .WithMany(p => p.BladeMeasurements)
                      .HasForeignKey(d => d.InspectionFormId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure InspectionAnalysis
            modelBuilder.Entity<InspectionAnalysis>(entity =>
            {
                entity.HasIndex(e => e.InspectionFormId)
                      .IsUnique()
                      .HasDatabaseName("IX_InspectionAnalysis_InspectionFormId");
                entity.HasIndex(e => e.Decision).HasDatabaseName("IX_InspectionAnalysis_Decision");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(d => d.InspectionForm)
                      .WithOne(p => p.InspectionAnalysis)
                      .HasForeignKey<InspectionAnalysis>(d => d.InspectionFormId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure MeasurementDeviation
            modelBuilder.Entity<MeasurementDeviation>(entity =>
            {
                entity.HasIndex(e => e.BladeMeasurementId).HasDatabaseName("IX_MeasurementDeviation_BladeMeasurementId");
                entity.HasIndex(e => e.MeasurementType).HasDatabaseName("IX_MeasurementDeviation_MeasurementType");
                entity.HasIndex(e => e.IsCritical).HasDatabaseName("IX_MeasurementDeviation_IsCritical");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(d => d.BladeMeasurement)
                      .WithMany(p => p.MeasurementDeviations)
                      .HasForeignKey(d => d.BladeMeasurementId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure ToleranceSpecification
            modelBuilder.Entity<ToleranceSpecification>(entity =>
            {
                entity.HasIndex(e => new { e.FormId, e.Revision, e.MeasurementType })
                      .IsUnique()
                      .HasDatabaseName("IX_ToleranceSpecification_FormId_Revision_MeasurementType");
                entity.HasIndex(e => e.IsActive).HasDatabaseName("IX_ToleranceSpecification_IsActive");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            // Configure InspectionAuditLog
            modelBuilder.Entity<InspectionAuditLog>(entity =>
            {
                entity.HasIndex(e => e.InspectionFormId).HasDatabaseName("IX_InspectionAuditLog_InspectionFormId");
                entity.HasIndex(e => e.CreatedAt).HasDatabaseName("IX_InspectionAuditLog_CreatedAt");
                entity.HasIndex(e => e.Action).HasDatabaseName("IX_InspectionAuditLog_Action");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(d => d.InspectionForm)
                      .WithMany(p => p.AuditLogs)
                      .HasForeignKey(d => d.InspectionFormId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Seed ToleranceSpecifications
            SeedToleranceSpecifications(modelBuilder);
        }

        private void SeedToleranceSpecifications(ModelBuilder modelBuilder)
        {
            var toleranceSpecs = new List<ToleranceSpecification>();
            var measurementTypes = new[] { "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "T1", "T2", "T3", "T4", "T5", "E1", "E2", "E3" };
            
            int id = 1;
            foreach (var measurementType in measurementTypes)
            {
                toleranceSpecs.Add(new ToleranceSpecification
                {
                    Id = id++,
                    FormId = "WIF-BKT-08",
                    Revision = "Rev1.0",
                    MeasurementType = measurementType,
                    NominalValue = 0.050m,
                    MinTolerance = 0.040m,
                    MaxTolerance = 0.060m,
                    RepairMinLimit = 0.030m,
                    RepairMaxLimit = 0.070m,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }

            modelBuilder.Entity<ToleranceSpecification>().HasData(toleranceSpecs);
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is InspectionForm || e.Entity is BladeMeasurement || 
                           e.Entity is InspectionAnalysis || e.Entity is ToleranceSpecification)
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    if (entry.Property("CreatedAt") != null)
                        entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
                }

                if (entry.Property("UpdatedAt") != null)
                    entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
            }
        }
    }
}