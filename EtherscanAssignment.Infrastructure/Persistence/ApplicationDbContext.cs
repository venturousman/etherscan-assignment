using EtherscanAssignment.Infrastructure.Persistence.Entities;
using System.Data.Entity;

namespace EtherscanAssignment.Infrastructure.Persistence
{
    //[DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class ApplicationDbContext : DbContext
    {
        // Your context has been configured to use a 'ApplicationModel' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'EtherscanAssignment.Infrastructure.Persistence.ApplicationModel' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'ApplicationModel' 
        // connection string in the application configuration file.
        public ApplicationDbContext()
            : base("DefaultConnectionString")
        {
            //Configuration.ValidateOnSaveEnabled = false;
            //Configuration.LazyLoadingEnabled = false;
            //DbConfiguration.SetConfiguration(new MySqlEFConfiguration());
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Token> Tokens { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //SetExecutionStrategy(MySqlProviderInvariantName.ProviderName, () => new MySqlExecutionStrategy());
            //    var init = new SqliteCreateDatabaseIfNotExists<ApplicationDbContext>(modelBuilder);
            //    Database.SetInitializer(init);

            modelBuilder.Entity<Token>().ToTable("token");

            modelBuilder.Entity<Token>().HasKey(t => t.Id);

            modelBuilder.Entity<Token>().Property(t => t.Symbol)
                .HasColumnName("symbol")
                .HasColumnType("varchar")
                .HasMaxLength(5)
                .IsRequired();

            modelBuilder.Entity<Token>().Property(t => t.Name)
                .HasColumnName("name")
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Token>().Property(t => t.TotalSupply)
                .HasColumnName("total_supply")
                .HasColumnType("bigint")
                .IsRequired();

            modelBuilder.Entity<Token>().Property(t => t.ContractAddress)
                .HasColumnName("contract_address")
                .HasColumnType("varchar")
                .HasMaxLength(66)
                .IsRequired();

            modelBuilder.Entity<Token>().Property(t => t.TotalHolders)
                .HasColumnName("total_holders")
                .HasColumnType("int")
                .IsRequired();

            modelBuilder.Entity<Token>().Property(t => t.Price)
                .HasColumnName("price")
                .HasColumnType("decimal")
                .IsOptional();

            base.OnModelCreating(modelBuilder);
        }
    }
}
