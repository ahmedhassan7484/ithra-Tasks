using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Commands
{
    public class CreateProductCommand:IRequest<productResponseDto>
    {
        public productResponseDto Product { get; set; }
        public CreateProductCommand(productResponseDto product)
        {
            Product = product;
        }
    }
}
