using FiapStore.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiapStore.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuario");

            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                .HasColumnType("INT")
                .UseIdentityColumn();

            builder.Property(u => u.Nome)
                .HasColumnType("varchar(100)");

            builder.Property(u => u.NomeUsuario)
                .HasColumnType("VARCHAR(50)")
                .IsRequired();

            builder.Property(u => u.Permissao)
                .HasConversion<int>() // caso queira salvar o texto da propriedade colocar HasConversion<string>, irá pegar o nome do enum ou a Description dele
                .IsRequired();

            builder.HasMany(u => u.Pedidos)
                .WithOne(p => p.Usuario)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
