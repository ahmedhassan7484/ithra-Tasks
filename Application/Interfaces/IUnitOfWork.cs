using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }

        IOrderRepository Orders { get; }
        IUserRepository Users { get; }

        Task<int> SaveChangesAsync();
    }
}
