using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoWakeCommerce.DTOs;
using WakeCommerceAPI.Data;
using WakeCommerceAPI.Models;

namespace ProjetoWakeCommerce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoDbContext _context;

        public ProdutoController(ProdutoDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Faz uma busca de produtos que contenham a string enviada no campo busca e pode ser ordenado baseado em valor ou estoque.
        /// </summary>
        /// <remarks>
        /// ```json
        /// {
        ///     "busca": "pro",
        ///     "orderBy": "estoque"
        /// }
        /// ```
        /// </remarks>
        /// <param name="busca">Filtro da busca por nome do produto.</param>
        /// <param name="orderBy">Critério de ordenação: valor ou estoque.</param>
        /// <returns>Retorna uma lista de produtos encontrados com base na busca e ordenação.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos([FromQuery] string? busca, [FromQuery] string? orderBy)
        {
            var produtos = _context.Produtos.AsQueryable();

            if (!string.IsNullOrEmpty(busca))
                produtos = produtos.Where(p => p.Nome.Contains(busca));

            if (!string.IsNullOrEmpty(orderBy))
                produtos = orderBy.ToLower() switch
                {
                    "valor" => produtos.OrderBy(p => p.Valor),
                    "estoque" => produtos.OrderBy(p => p.Estoque),
                    _ => produtos.OrderBy(p => p.Nome)
                };

            return await produtos.ToListAsync();
        }

        /// <summary>
        /// Faz a busca de um produto pelo Id
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição HTTP:
        /// 
        /// GET /Produto/{id}
        /// 
        /// Corpo da requisição (JSON):
        /// ```json
        /// {
        ///     "id": 1
        /// }
        /// ```
        /// </remarks>
        /// <param name="id">Id do produto a ser buscado</param>
        /// <returns>Retorna o produto solicitado</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> Getproduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) return NotFound();
            return produto;
        }

        /// <summary>
        /// Cria um novo produto.
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição HTTP:
        /// 
        /// POST /Produto
        /// 
        /// Corpo da requisição (JSON):
        /// ```json
        /// {
        ///     "id": 0,
        ///     "nome": "Produto Exemplo",
        ///     "estoque": 10,
        ///     "valor": 100
        /// }
        /// ```
        /// </remarks>
        /// <param name="dto">Objeto de transferência de dados - contém os campos necessários para envio</param>
        /// <returns>Retorna o produto criado</returns>
        [HttpPost]
        public async Task<ActionResult<Produto>> CreateProduto(CreateProdutoDTO dto)
        {
            if (dto.Valor < 0) return BadRequest("Preço não pode ser negativo.");

            var produto = new Produto
            {
                Nome = dto.Nome,
                Estoque = dto.Estoque,
                Valor = dto.Valor
            };

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Getproduto), new { id = produto.Id }, produto);
        }

        /// <summary>
        /// Atualiza um produto.
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição HTTP:
        /// 
        /// PUT /Produto
        /// 
        /// Corpo da requisição (JSON):
        /// ```json
        /// {
        ///     "id": 0,
        ///     "nome": "Produto Atualizado",
        ///     "estoque": 15,
        ///     "valor": 120
        /// }
        /// ```
        /// </remarks>
        /// <param name="dto">Objeto de transferência de dados - contém os campos necessários para envio</param>
        /// <returns>Retorna 204 No Content</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateProduto(UpdateProdutoDTO dto)
        {
            var produto = await _context.Produtos.FindAsync(dto.Id);
            if (produto == null) return NotFound();

            if (dto.Nome != null) produto.Nome = dto.Nome;
            if (dto.Estoque.HasValue) produto.Estoque = dto.Estoque.Value;
            if (dto.Valor.HasValue) produto.Valor = dto.Valor.Value;

            if (produto.Valor < 0) return BadRequest("Preço não pode ser negativo.");

            _context.Entry(produto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Deleta um produto
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição HTTP:
        /// 
        /// DELETE /Produto/{id}
        /// 
        /// Corpo da requisição (JSON):
        /// ```json
        /// {
        ///     "id": 1
        /// }
        /// ```
        /// </remarks>
        /// <param name="id">Id do produto a ser deletado</param>
        /// <returns>Retorna 204 No Content</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) return NotFound();
            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
