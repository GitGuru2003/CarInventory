using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using CarInventory.Models;


namespace CarInventory.Data
{
  class DataContextEF : DbContext
  {
    private string _connectionString;
    public DataContextEF(IConfiguration config)
    {
      _connectionString = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    public DbSet<Car> Cars { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
      if (!options.IsConfigured)
      {
        options.UseSqlServer(_connectionString, (options) => options.EnableRetryOnFailure());
      }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.HasDefaultSchema("InventorySchema");
      modelBuilder.Entity<Car>().ToTable("Cars");
      modelBuilder.Entity<Car>().HasKey(c => c.CarId);
    }
  }
}