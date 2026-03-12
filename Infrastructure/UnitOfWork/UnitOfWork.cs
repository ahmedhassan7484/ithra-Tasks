using Application.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IthraDbContext _context;
        public IProductRepository Products { get; set; }
        public IUserRepository Users { get; set; }
        public IOrderRepository Orders { get; set; }
        public UnitOfWork(
            IthraDbContext context,
            IProductRepository productRepository,
            IUserRepository users,
            IOrderRepository orderRepository
            )
        
        {
            _context = context;
            Products = productRepository;
            Orders = orderRepository;
            Users = users;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
