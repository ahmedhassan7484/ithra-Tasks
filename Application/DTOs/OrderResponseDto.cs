using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class OrderResponseDto
    {
        public int OrderId { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal Discount { get; set; }

        public List<OrderItemResponseDto> Items { get; set; } = new();
    }
}
