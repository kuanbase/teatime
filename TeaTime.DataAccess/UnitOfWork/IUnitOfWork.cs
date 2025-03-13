using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeaTime.DataAccess.Repository;

namespace TeaTime.DataAccess.UnitOfWork
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }

        IShoppingCartRepository ShoppingCart { get; }
        void Save();
    }
}
