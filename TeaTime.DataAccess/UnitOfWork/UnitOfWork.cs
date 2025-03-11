using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeaTime.DataAccess.Data;
using TeaTime.DataAccess.Repository;

namespace TeaTime.DataAccess.UnitOfWork
{
    public class UnitOfWork: IUnitOfWork
    {
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context) 
        {
            _context = context;
            Category = new CategoryRepository(_context);
            Product = new ProdcutRepository(_context);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
