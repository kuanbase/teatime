using Microsoft.EntityFrameworkCore;
using TeaTime.DataAccess.Data;
using TeaTime.Models;

namespace TeaTime.DataAccess.Repository
{
    public class ProdcutRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProdcutRepository(ApplicationDbContext context): base(context)
        {
            _context = context;
        }

        public void Update(Product obj)
        {
            _context.Products.Update(obj);
        }
    }
}
