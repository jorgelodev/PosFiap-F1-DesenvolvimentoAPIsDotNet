namespace FiapStore.Entity
{
    public class Pedido : Entidade
    {


        public string NomeProduto { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public decimal PrecoTotal { get; set; }

        public Pedido()
        {

        }
        public Pedido(Pedido pedido)
        {
            NomeProduto = pedido.NomeProduto;
            UsuarioId = pedido.UsuarioId;
            PrecoTotal = pedido.PrecoTotal;
        }

    }
}
