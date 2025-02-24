using Microsoft.EntityFrameworkCore;
using WakeCommerceAPI.Models;

namespace WakeCommerceAPI.Data
{
    public class ProdutoDbContext : DbContext
    {
        public ProdutoDbContext(DbContextOptions<ProdutoDbContext> options) : base(options) { }
        public DbSet<Produto> Produtos { get; set; }
    }
}
