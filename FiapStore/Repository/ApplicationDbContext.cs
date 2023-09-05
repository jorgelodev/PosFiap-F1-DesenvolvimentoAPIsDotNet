using FiapStore.Configurations;
using FiapStore.Entity;
using Microsoft.EntityFrameworkCore;

namespace FiapStore.Repository
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetValue<string>("ConnectionStrings:ConnectionString"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            //modelBuilder.ApplyConfiguration(new PedidoConfiguration());
            // para não ser necessário inserir sempre as configurãções das entidades
            // é possível realizar da forma abaixo
            // ele vai pegar todas as entidades que implementam IEntityTypeConfiguration
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
