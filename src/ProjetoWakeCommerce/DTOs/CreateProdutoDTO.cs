namespace ProjetoWakeCommerce.DTOs
{
    public class CreateProdutoDTO
    {
        public string Nome { get; set; } = string.Empty;
        public int Estoque { get; set; }
        public decimal Valor { get; set; }
    }
}
