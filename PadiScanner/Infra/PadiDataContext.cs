using Microsoft.EntityFrameworkCore;
using PadiScanner.Data;
using PadiScanner.Infra.Converters;

namespace PadiScanner.Infra;

public class PadiDataContext : DbContext
{
    public PadiDataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<PredictionHistory> Predictions => Set<PredictionHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // predictions entity
        modelBuilder.Entity<PredictionHistory>()
            .Property(x => x.Id)
            .HasMaxLength(26)
            .HasConversion<UlidConverter>();
        modelBuilder.Entity<PredictionHistory>()
            .Property(x => x.Probabilities)
            .HasConversion<ProbabilitiesConverter>();
        modelBuilder.Entity<PredictionHistory>()
            .HasOne(x => x.Uploader)
            .WithMany(x => x.Predictions)
            .HasForeignKey(x => x.UploaderId);

        // users entity
        modelBuilder.Entity<User>()
            .Property(x => x.Id)
            .HasMaxLength(26)
            .HasConversion<UlidConverter>();

        // ---- data seeding
        var userDataSeed = new List<User>
        {
            new()
            {
                Id = Ulid.Parse("01GE24HFHHQZRRN024W32W8XF7"),
                FullName = "NyankoAdmin",
                Username = "nynanko",
                Password = BCrypt.Net.BCrypt.HashPassword("sensei"),
                Role = UserRole.Administrator,
                LastLoginAt = DateTime.Now,
            },
            new()
            {
                Id = Ulid.Parse("01GE24MT8165ZNXACDZYC8GMEQ"),
                FullName = "Tamu",
                Username = "tamu",
                Password = BCrypt.Net.BCrypt.HashPassword("tamu"),
                Role = UserRole.Guest,
                LastLoginAt = DateTime.Now,
            }
        };

        modelBuilder.Entity<User>()
            .HasData(userDataSeed);
    }
}
