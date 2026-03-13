using Application.Common;
using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Queries
{
    public class GetProductsQuery:IRequest<PageResult<productResponseDto>>
    {
        public ProductFilterDto Filter { get; set; }

        public GetProductsQuery(ProductFilterDto filter)
        {
            Filter = filter;
        }
    }
}
