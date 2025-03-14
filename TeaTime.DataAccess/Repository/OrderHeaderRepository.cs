using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TeaTime.DataAccess.Data;
using TeaTime.Models;

namespace TeaTime.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderHeaderRepository(ApplicationDbContext context): base(context)
        {
            _context = context;
        }
        public void Update(OrderHeader obj)
        {
            _context.OrderHeaders.Update(obj);
        }
    }
}
