namespace ProjetoWakeCommerce.DTOs
{
    public class UpdateProdutoDTO
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public int? Estoque { get; set; }
        public decimal? Valor { get; set; }
    }
}
