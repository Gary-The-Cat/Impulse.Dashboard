namespace Impulse.Repository.Persistent;

using Microsoft.EntityFrameworkCore;

internal sealed class PluginDataContext : DbContext
{
    private readonly string connectionString;

    public PluginDataContext(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public DbSet<StoredSingletonModel> SingletonModels => Set<StoredSingletonModel>();

    public DbSet<StoredCollectionModel> CollectionModels => Set<StoredCollectionModel>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var singletonEntity = modelBuilder.Entity<StoredSingletonModel>();
        singletonEntity.ToTable("PluginSingletons");
        singletonEntity.HasKey(e => e.Id);
        singletonEntity.Property(e => e.TypeName).IsRequired();
        singletonEntity.Property(e => e.Payload).IsRequired();
        singletonEntity.Property(e => e.UpdatedOn).IsRequired();
        singletonEntity.HasIndex(e => e.TypeName).IsUnique();

        var collectionEntity = modelBuilder.Entity<StoredCollectionModel>();
        collectionEntity.ToTable("PluginCollections");
        collectionEntity.HasKey(e => e.Id);
        collectionEntity.Property(e => e.TypeName).IsRequired();
        collectionEntity.Property(e => e.Payload).IsRequired();
        collectionEntity.Property(e => e.Position).IsRequired();
        collectionEntity.Property(e => e.CreatedOn).IsRequired();
        collectionEntity.HasIndex(e => new { e.TypeName, e.Position }).IsUnique();
    }
}
