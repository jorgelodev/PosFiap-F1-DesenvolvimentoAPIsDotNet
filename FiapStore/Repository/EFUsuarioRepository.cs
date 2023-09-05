using FiapStore.Entity;
using FiapStore.Interface;
using Microsoft.EntityFrameworkCore;

namespace FiapStore.Repository
{
    public class EFUsuarioRepository : EFRepository<Usuario>, IUsuarioRepository
    {
        public EFUsuarioRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Usuario ObterComPedidos(int id)
        {
            return _context.Usuarios
                .Include(u => u.Pedidos)
                .Where(u => u.Id == id)
                .ToList()
                .Select(usuario =>
                {
                    usuario.Pedidos = usuario.Pedidos.Select(pedido => new Pedido(pedido)).ToList();
                    return usuario;
                }).FirstOrDefault();
        }

        public Usuario ObterPorNomeUsuarioESenha(string nomeUsuario, string senha)
        {
            return _context.Usuarios.FirstOrDefault(u => u.NomeUsuario.Equals(nomeUsuario) && u.Senha.Equals(senha));
        }
    }
}
