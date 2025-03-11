using Microsoft.EntityFrameworkCore;
using TeaTime.Models;

namespace TeaTime.DataAccess.Repository
{
    public interface IProductRepository: IRepository<Product>
    {
        void Update(Product obj);
    }
}
