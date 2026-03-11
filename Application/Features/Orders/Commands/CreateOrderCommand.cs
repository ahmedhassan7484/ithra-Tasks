using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Commands
{
    public class CreateOrderCommand:IRequest<OrderResponseDto>
    {
        public CreateOrderRequestDto Order { get; set; }
        public CreateOrderCommand(CreateOrderRequestDto order)
        {
            Order = order;
        }
    }
}
