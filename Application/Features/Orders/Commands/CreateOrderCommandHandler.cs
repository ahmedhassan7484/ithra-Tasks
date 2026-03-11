using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Commands
{
    public class CreateOrderCommandHandler:IRequestHandler<CreateOrderCommand, OrderResponseDto>
    {
        //private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IOrderRepository _orderRepository;

        public CreateOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            //_productRepository = productRepository;
            //_orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderResponseDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            decimal total = 0;

            var order = new Order
            {
                UserId = request.Order.UserId
            };

            foreach (var item in request.Order.Items)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);

                if (product == null)
                    throw new Exception("Product not found");

                if (product.StockQuantity < item.Quantity)
                    throw new Exception($"Insufficient stock for {product.Name}");

                product.StockQuantity -= item.Quantity;

                await _unitOfWork.Products.UpdateAsync(product);

                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                };

                total += product.Price * item.Quantity;

                order.Items.Add(orderItem);
            }

            decimal discount = 0;

            if (total > 500)
                discount = total * 0.10m;

            order.TotalAmount = total - discount;
            order.Discount = discount;

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();   
            return new OrderResponseDto
            {
                OrderId = order.Id,
                TotalAmount = order.TotalAmount,
                Discount = discount,
                Items = order.Items.Select(i => new OrderItemResponseDto
                {
                    ProductName = "",
                    Quantity = i.Quantity,
                    Price = i.UnitPrice
                }).ToList()
            };
        }
    }
}
