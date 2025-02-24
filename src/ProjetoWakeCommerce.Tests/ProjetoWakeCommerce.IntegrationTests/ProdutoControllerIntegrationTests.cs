using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using WakeCommerceAPI.Models;
using ProjetoWakeCommerce.DTOs;
using System.Net;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace ProjetoWakeCommerce.Tests.IntegrationTests
{
    public class ProdutoControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProdutoControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetProdutos_ReturnsOkResult()
        {
            var response = await _client.GetAsync("/Produto");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task CreateProduto_ReturnsCreated()
        {
            var newProduct = new CreateProdutoDTO { Nome = "Produto Teste", Estoque = 10, Valor = 99.99m };
            var response = await _client.PostAsJsonAsync("/Produto", newProduct);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task GetProduto_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/Produto/999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateProduto_ReturnsNoContent()
        {
            var newProduct = new CreateProdutoDTO { Nome = "Produto Update", Estoque = 5, Valor = 50.00m };
            var createdResponse = await _client.PostAsJsonAsync("/Produto", newProduct);
            var createdProduct = await createdResponse.Content.ReadFromJsonAsync<Produto>();

            var updateDto = new UpdateProdutoDTO { Id = createdProduct.Id, Nome = "Produto Atualizado", Estoque = 20, Valor = 150.00m };
            var response = await _client.PutAsJsonAsync("/Produto", updateDto);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteProduto_ReturnsNoContent()
        {
            var newProduct = new CreateProdutoDTO { Nome = "Produto Delete", Estoque = 8, Valor = 30.00m };
            var createdResponse = await _client.PostAsJsonAsync("/Produto", newProduct);
            var createdProduct = await createdResponse.Content.ReadFromJsonAsync<Produto>();

            var response = await _client.DeleteAsync($"/Produto/{createdProduct.Id}");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }

}