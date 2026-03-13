using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using ithra_backend.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;
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
        private readonly IHubContext<NotificationHub> _hub;

        public CreateOrderCommandHandler(IUnitOfWork unitOfWork, IHubContext<NotificationHub> hub)
        {
            //_productRepository = productRepository;
        
            _unitOfWork = unitOfWork;
            _hub = hub;
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
                {
                    await _hub.Clients.User(order.UserId.ToString()).SendAsync("ReceiveNotification", $"Product not found");
                    throw new Exception("Product not found");
                }

                if (product.StockQuantity < item.Quantity)
                {
                    await _hub.Clients.User(order.UserId.ToString()).SendAsync("ReceiveNotification", $"Insufficient stock for {product.Name}");
                    throw new Exception($"Insufficient stock for {product.Name}");
                }

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
            //send notification 
            await _hub.Clients.All.SendAsync("ReceiveNotification", $"New order created with ID: {order.Id}");

            return new OrderResponseDto
            {
                OrderId = order.Id,
                TotalAmount = order.TotalAmount,
                Discount = discount,
                Items = order.Items.Select(i => new OrderItemResponseDto
                {
                    ProductName = i.Product.Name,
                    Quantity = i.Quantity,
                    Price = i.UnitPrice
                }).ToList()
            };
        }
    }
}
