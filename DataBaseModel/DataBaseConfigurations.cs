using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseModel
{
    public sealed class InitialOptionsMonitor<TOption> : IOptionsMonitor<TOption> where TOption : class, new()
    {
        public TOption CurrentValue { get; private set; } = default!;
        public InitialOptionsMonitor(TOption currentValue) => this.CurrentValue = currentValue;

        TOption IOptionsMonitor<TOption>.Get(string? name) => this.CurrentValue;
        public IDisposable OnChange(Action<TOption, string> listener) { return default!; }
    }

    internal partial class DatabaseContextFactory : IDesignTimeDbContextFactory<KursProjectDataBaseContext>
    {
        public DatabaseContextFactory() : base() { }
        public interface DatabaseOptionsConfigure<TContext> where TContext : DbContext
        {
            public abstract DbContextOptionsBuilder<TContext> ContextOptions { get; set; }
            public abstract DbContextOptions<TContext> ConfigureOptions(System.String config_name);
        }
        public partial class DatabaseConfigure : DatabaseOptionsConfigure<KursProjectDataBaseContext>
        {
            public DbContextOptionsBuilder<KursProjectDataBaseContext> ContextOptions { get; set; } = default!;
            public DatabaseConfigure(DbContextOptionsBuilder<KursProjectDataBaseContext> options) : base() => this.ContextOptions = options;

            public DbContextOptions<KursProjectDataBaseContext> ConfigureOptions(string config_name = "config.json")
            {
                var builder = new ConfigurationBuilder();
                var config = builder.SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile(config_name).Build();

                return this.ContextOptions.UseNpgsql(config.GetConnectionString("DefaultConnection")).Options;
            }
        }

        public virtual KursProjectDataBaseContext CreateDbContext(string[] args)
        {
            var logger_option = new InitialOptionsMonitor<DataBaseLoggerConfiguration>(new DataBaseLoggerConfiguration());
            var options_builder = new DbContextOptionsBuilder<KursProjectDataBaseContext>();

            var database_options = (new DatabaseConfigure(options_builder)).ConfigureOptions();
            return new KursProjectDataBaseContext(database_options, logger_option);
        }

    }
}
