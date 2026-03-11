using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Commands
{
    public class CreateProductCommandHandler:IRequestHandler<CreateProductCommand, productResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<productResponseDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Product.Name,
                Price = request.Product.Price,
                StockQuantity = request.Product.Stock,
                Category = request.Product.Category
            };

            await _unitOfWork.Products.AddAsync(product);

            await _unitOfWork.SaveChangesAsync();

            return new productResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.StockQuantity,
                Category = product.Category
            };
        }
    }
}
