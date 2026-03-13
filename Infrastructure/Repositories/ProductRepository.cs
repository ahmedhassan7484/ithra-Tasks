using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{

    public class ProductRepository: IProductRepository
    {
        private readonly IthraDbContext _context;
        public ProductRepository(IthraDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public async Task<IQueryable<Product>> GetAllAsync()
        {
            return _context.Products.AsQueryable();
        }
        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
           // await _context.SaveChangesAsync();
        }
    }
}
