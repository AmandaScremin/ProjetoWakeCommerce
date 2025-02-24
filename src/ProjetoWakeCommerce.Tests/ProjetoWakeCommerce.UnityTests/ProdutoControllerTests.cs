using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoWakeCommerce.DTOs;
using WakeCommerceAPI.Data;
using WakeCommerceAPI.Models;
using ProjetoWakeCommerce.Controllers;

namespace ProjetoWakeCommerce.Tests
{
    public class ProdutoControllerTests
    {
        private readonly ProdutoController _controller;
        private readonly ProdutoDbContext _context;

        public ProdutoControllerTests()
        {
            var options = new DbContextOptionsBuilder<ProdutoDbContext>()
                .UseInMemoryDatabase(databaseName: "BDTeste")
                .Options;

            _context = new ProdutoDbContext(options);
            _controller = new ProdutoController(_context);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        #region GetProdutos
        [Fact]
        public async Task GetProdutos_ReturnsOkResult_WhenProductsExist()
        {
            var produto1 = new Produto { Nome = "Produto A", Estoque = 10, Valor = 100.0m };
            var produto2 = new Produto { Nome = "Produto B", Estoque = 20, Valor = 200.0m };
            _context.Produtos.Add(produto1);
            _context.Produtos.Add(produto2);
            await _context.SaveChangesAsync();

            var result = await _controller.GetProdutos(null, null);

            var actionResult = Assert.IsType<ActionResult<IEnumerable<Produto>>>(result);
            var produtos = Assert.IsAssignableFrom<List<Produto>>(actionResult.Value);
            Assert.Equal(2, produtos.Count);
        }
        #endregion

        #region GetProduto
        [Fact]
        public async Task GetProduto_ReturnsNotFound_WhenProductDoesNotExist()
        {
            var result = await _controller.Getproduto(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetProduto_ReturnsOkResult_WhenProductExists()
        {
            var produto = new Produto { Nome = "Produto A", Estoque = 10, Valor = 100.0m };
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            var result = await _controller.Getproduto(produto.Id);

            var actionResult = Assert.IsType<ActionResult<Produto>>(result);
            var returnedProduto = Assert.IsType<Produto>(actionResult.Value);
            Assert.Equal(produto.Nome, returnedProduto.Nome);
        }
        #endregion

        #region CreateProduto
        [Fact]
        public async Task CreateProduto_ReturnsCreatedAtActionResult_WhenProductIsValid()
        {
            var productDTO = new CreateProdutoDTO
            {
                Nome = "Produto A",
                Estoque = 10,
                Valor = 100.0m
            };

            var result = await _controller.CreateProduto(productDTO);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdProduct = Assert.IsType<Produto>(createdAtActionResult.Value);
            Assert.Equal("Produto A", createdProduct.Nome);
            Assert.Equal(10, createdProduct.Estoque);
            Assert.Equal(100.0m, createdProduct.Valor);
        }

        [Fact]
        public async Task CreateProduto_ReturnsBadRequest_WhenPriceIsNegative()
        {
            var productDTO = new CreateProdutoDTO
            {
                Nome = "Produto A",
                Estoque = 10,
                Valor = -1.0m
            };

            var result = await _controller.CreateProduto(productDTO);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Preço não pode ser negativo.", badRequestResult.Value);
        }
        #endregion

        #region UpdateProduto
        [Fact]
        public async Task UpdateProduto_ReturnsNoContent_WhenProductIsUpdated()
        {
            var produto = new Produto { Id = 1, Nome = "Produto A", Estoque = 10, Valor = 100.0m };
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            var productUpdateDTO = new UpdateProdutoDTO
            {
                Id = 1,
                Nome = "Produto Atualizado",
                Estoque = 20,
                Valor = 150.0m
            };

            var result = await _controller.UpdateProduto(productUpdateDTO);

            Assert.IsType<NoContentResult>(result);

            var updatedProduct = await _context.Produtos.FindAsync(1);
            Assert.Equal("Produto Atualizado", updatedProduct.Nome);
            Assert.Equal(20, updatedProduct.Estoque);
            Assert.Equal(150.0m, updatedProduct.Valor);
        }

        [Fact]
        public async Task UpdateProduto_ReturnsBadRequest_WhenPriceIsNegative()
        {
            var produto = new Produto { Id = 1, Nome = "Produto A", Estoque = 10, Valor = 100.0m };
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            var productUpdateDTO = new UpdateProdutoDTO
            {
                Id = 1,
                Nome = "Produto Atualizado",
                Estoque = 20,
                Valor = -1.0m
            };

            var result = await _controller.UpdateProduto(productUpdateDTO);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateProduto_ReturnsNotFound_WhenProductDoesNotExist()
        {
            var productUpdateDTO = new UpdateProdutoDTO
            {
                Id = 1,
                Nome = "Produto Atualizado",
                Estoque = 20,
                Valor = 150.0m
            };

            var result = await _controller.UpdateProduto(productUpdateDTO);

            Assert.IsType<NotFoundResult>(result);
        }
        #endregion

        #region DeleteProduto
        [Fact]
        public async Task DeleteProduto_ReturnsNoContent_WhenProductIsDeleted()
        {
            var produto = new Produto { Id = 1, Nome = "Produto A", Estoque = 10, Valor = 100.0m };
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            var result = await _controller.DeleteProduto(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteProduto_ReturnsNotFound_WhenProductDoesNotExist()
        {
            var result = await _controller.DeleteProduto(1);

            Assert.IsType<NotFoundResult>(result);
        }
        #endregion
    }
}
