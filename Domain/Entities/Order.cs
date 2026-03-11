using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order:Base
    {
       

        public int UserId { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal Discount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ApplicationUser? User { get; set; }
        public List<OrderItem> Items { get; set; } = new();
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
    }
}
