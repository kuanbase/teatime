using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeaTime.Models;

namespace TeaTime.DataAccess.Repository;

public interface IOrderHeaderRepository: IRepository<OrderHeader>
{
    void Update(OrderHeader obj);
}
