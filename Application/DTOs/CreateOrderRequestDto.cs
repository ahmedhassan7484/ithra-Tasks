using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateOrderRequestDto
    {
        public int UserId { get; set; }

        public List<OrderItemRequestDto> Items { get; set; } = new();
    }
}
