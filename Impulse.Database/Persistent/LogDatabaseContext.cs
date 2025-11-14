namespace Impulse.Repository.Persistent;

using Impulse.Repository.Models;
using Microsoft.EntityFrameworkCore;

internal sealed class LogDatabaseContext : DbContext
{
    private readonly string connectionString;

    public LogDatabaseContext(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public DbSet<LogRecordModel> LogRecords => Set<LogRecordModel>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var logRecord = modelBuilder.Entity<LogRecordModel>();
        logRecord.ToTable("LogRecords");
        logRecord.HasKey(r => r.Id);
        logRecord.Property(r => r.Id)
            .IsRequired()
            .ValueGeneratedNever();
        logRecord.Property(r => r.Message).IsRequired();
        logRecord.Property(r => r.Timestamp).IsRequired();
        logRecord.Property(r => r.Criticality).IsRequired();

        logRecord
            .HasDiscriminator<string>("RecordType")
            .HasValue<InfoLogRecordModel>("Info")
            .HasValue<WarningLogRecordModel>("Warning")
            .HasValue<ErrorLogRecordModel>("Error")
            .HasValue<ExceptionLogRecordModel>("Exception");

        modelBuilder.Entity<ExceptionLogRecordModel>(entity =>
        {
            entity.Property(e => e.StackTrace).HasDefaultValue(string.Empty);
            entity.Property(e => e.ExceptionType).HasDefaultValue(string.Empty);
            entity.Property(e => e.ExceptionMessage).HasDefaultValue(string.Empty);
        });
    }
}
