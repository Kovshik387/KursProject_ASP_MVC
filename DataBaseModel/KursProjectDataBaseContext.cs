using DataBaseModel.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using static DataBaseModel.DataBaseLoggerConfiguration;
using Microsoft.Extensions.Configuration;

namespace DataBaseModel;

using LoggerConfiguration = IOptionsMonitor<DataBaseLoggerConfiguration>;
public partial class KursProjectDataBaseContext : DbContext
{
    protected static ILoggerFactory DatabaseLoggerFactory = default(ILoggerFactory)!;
    public KursProjectDataBaseContext(LoggerConfiguration configuration) : base() { base.Database.EnsureCreated(); LoggerInitial(configuration); }

    [ActivatorUtilitiesConstructorAttribute()]
    public KursProjectDataBaseContext(DbContextOptions<KursProjectDataBaseContext> options, LoggerConfiguration configuration) : base(options)
    { base.Database.EnsureCreated(); this.LoggerInitial(configuration); }
    public virtual DbSet<Authorization> Authorizations { get; set; }

    public virtual DbSet<Authorizationtype> Authorizationtypes { get; set; }

    public virtual DbSet<Contract> Contracts { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Placement> Placements { get; set; }

    public virtual DbSet<Placementtype> Placementtypes { get; set; }

    public virtual DbSet<Renter> Renters { get; set; }

    public virtual DbSet<Solution> Solutions { get; set; }

    public virtual DbSet<Tenant> Tenants { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected void LoggerInitial(IOptionsMonitor<DataBaseLoggerConfiguration> configuration)
    {
        KursProjectDataBaseContext.DatabaseLoggerFactory = LoggerFactory.Create((ILoggingBuilder builder) =>
        {
            builder.AddFilter((category, level) => category == DbLoggerCategory.Database.Command.Name)
                .AddProvider(new DatabaseLoggerProvider(configuration));
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(KursProjectDataBaseContext.DatabaseLoggerFactory);
        if (optionsBuilder.IsConfigured == true) return;

        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=KursProjectDataBase;Username=postgres;Password=123");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) => OnModelCreatingPartial(modelBuilder);

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
