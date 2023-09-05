using FiapStore.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiapStore.Configurations
{
    public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedido");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .HasColumnType("int")
                .UseIdentityColumn();
            builder.Property(p => p.NomeProduto).HasColumnType("varchar(100)");
            builder.Property(p => p.PrecoTotal).HasColumnType("decimal");
            builder.HasOne(p => p.Usuario)
                .WithMany(u => u.Pedidos)
                .HasPrincipalKey(u => u.Id);
        }
    }
}
