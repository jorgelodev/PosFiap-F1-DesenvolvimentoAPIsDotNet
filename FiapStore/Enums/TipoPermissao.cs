using System.ComponentModel;

namespace FiapStore.Enums
{
    public enum TipoPermissao
    {
        [Description("Administrador")]
        Administrador = 1,
        [Description("Funcionario")]
        Funcionario = 2
    }

    public static class Permissoes
    {
        public const string Administrador = "Administrador";
        public const string Funcionario = "Funcionario";
    }
}
