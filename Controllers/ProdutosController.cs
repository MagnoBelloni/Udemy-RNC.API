using Microsoft.AspNetCore.Mvc;
using RNC.API.Data;

namespace RNC.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private IProdutoRepository Repositorio;
        public ProdutosController(IProdutoRepository repositorio)
        {
            Repositorio = repositorio;
        }

        [HttpPost]
        [ApiVersion("1.0")]
        public IActionResult Criar([FromBody]Produto produto)
        {
            if (produto.Codigo == "")
                return BadRequest("Código do produto não informado!");

            if (string.IsNullOrWhiteSpace(produto.Descricao))
                return (BadRequest("Campo descrição vazio"));

            Repositorio.Inserir(produto);

            return Created(nameof(Criar), produto);
        }

        [HttpGet]
        [ApiVersion("1.0")]
        [ResponseCache(Duration = 30)]
        public IActionResult Obter()
        {
            var lista = Repositorio.Obter();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        [ApiVersion("1.0")]
        public IActionResult Obter(int id)
        {
            var produto = Repositorio.Obter(id);

            if (produto == null)
                return NotFound("Produto não encontrado!");

            return Ok(produto);
        }

        [HttpPut]
        [ApiVersion("1.0")]
        public IActionResult Atualizar([FromBody]Produto produto)
        {
            var prod = Repositorio.Obter(produto.Id);

            if (prod == null) return NotFound();

            if (produto.Codigo == "")
                return BadRequest("Código do produto não informado!");

            if (string.IsNullOrWhiteSpace(produto.Descricao))
                return (BadRequest("campo descrição vazio"));

            Repositorio.Editar(produto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ApiVersion("1.0")]
        public IActionResult Apagar(int id)
        {
            var produto = Repositorio.Obter(id);

            if (produto == null)
                return NotFound();

            Repositorio.Excluir(produto);

            return Ok();
        }

        [HttpGet("{codigo}")]
        [ApiVersion("2.0")]
        public IActionResult ObterPorCodigo(string codigo)
        {
            return Ok("Método obter por código versão 2 --Beta");
        }
    }
}