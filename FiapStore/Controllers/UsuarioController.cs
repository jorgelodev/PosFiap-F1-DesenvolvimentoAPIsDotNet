using FiapStore.DTO;
using FiapStore.Entity;
using FiapStore.Enums;
using FiapStore.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiapStore.Controllers
{
    [ApiController]
    [Route("Usuario")]
    public class UsuarioController : ControllerBase
    {
        private IUsuarioRepository _usuarioRepository;
        private ILogger<UsuarioController> _logger;


        public UsuarioController(IUsuarioRepository usuarioRepository, ILogger<UsuarioController> logger)
        {
            _usuarioRepository = usuarioRepository;
            _logger = logger;
        }
                
        [Authorize]
        [Authorize(Roles = Permissoes.Administrador)]
        [HttpGet("obter-todos-usuarios")]
        public IActionResult ObterTodosUsuarios()
        {
            try
            {
                //throw new Exception("Deu erro");
                return Ok(_usuarioRepository.ObterTodos());
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Exception Forçada: {ex.Message}");
                return BadRequest();                   
                    
            }
        }

        /// <summary>
        /// Obtém todos os usuários com os respectivos pedidos.
        /// </summary>
        /// <param name="id">Id do usuário</param>
        /// <returns>Retorna uma lista de usuários com os respectivos pedidos preenchidos</returns>
        /// <remarks>
        /// Exemplo:
        /// 
        /// Enviar Id para requisição
        /// </remarks>
        /// <response code="200">Retorna Sucesso</response>
        /// <response code="401">Não Autenticado</response>
        /// <response code="403">Não Autorizado</response>
        [Authorize]
        [HttpGet("obter-com-pedidos/{id}")]
        public IActionResult ObterComPedidos([FromRoute] int id)
        {
            return Ok(_usuarioRepository.ObterComPedidos(id));
        }

        [Authorize]
        [Authorize(Roles = Permissoes.Funcionario)]
        [HttpGet("obter-por-usuario-id/{id}")]
        public IActionResult ObterPorUsuarioId(int id)
        {
            _logger.LogInformation("Executando método ObterPorUsuarioId");
            return Ok(_usuarioRepository.ObterPorId(id));
        }

        [HttpPost]
        [Authorize]
        [Authorize(Roles = $"{Permissoes.Funcionario} , {Permissoes.Administrador}")]
        // na versao .net 7.0 não precisa informar o [FromBody]
        public IActionResult CadastrarUsuario(CadastrarUsuarioDTO cadastrarUsuarioDTO)
        {
            _usuarioRepository.Cadastrar(new Usuario(cadastrarUsuarioDTO));

            var mensagem = $"Usuário criado com sucesso! | Nome : {cadastrarUsuarioDTO.Nome}";

            _logger.LogWarning(mensagem);

            return Ok(mensagem);
        }

        [HttpPut]
        public IActionResult AlterarUsuario(AlterarUsuarioDTO alterarUsuarioDTO)
        {
            _usuarioRepository.Alterar(new Usuario(alterarUsuarioDTO));
            return Ok("Usuário alterado com sucesso!");
        }

        [HttpDelete]
        public IActionResult DeletarUsuario(int id)
        {
            _usuarioRepository.Deletar(id);
            return Ok("Usuário deletado com sucesso!");
        }
    }
}
