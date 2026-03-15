using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Queries
{
    public class GetProductsQueryHandler:IRequestHandler<GetProductsQuery,PageResult<productResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProductsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PageResult<productResponseDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var query = await _unitOfWork.Products.GetAllAsync();

            // Search
            if (!string.IsNullOrEmpty(request.Filter.Search))
            {
                query =  query.Where(p => p.Name.Contains(request.Filter.Search));
            }

            // Filter by Category
            if (!string.IsNullOrEmpty(request.Filter.Category))
            {
                query = query.Where(p => p.Category == request.Filter.Category);
            }

            // Sorting
            if (!string.IsNullOrEmpty(request.Filter.SortBy))
            {
                if (request.Filter.SortBy.ToLower() == "desc")
                {
                    query = request.Filter.SortDirection == "desc"
                        ? query.OrderByDescending(p => p.Price)
                        : query.OrderBy(p => p.Price);
                }
            }

            // Pagination
            var products =  query
                .Skip((request.Filter.Page - 1) * request.Filter.PageSize)
                .Take(request.Filter.PageSize)
                .Select(p => new productResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Category = p.Category
                })
                .AsEnumerable();
            var totalCount =  query.Count();

            return new PageResult<productResponseDto>
            {
                Page = request.Filter.Page,
                PageSize = request.Filter.PageSize,
                TotalCount = totalCount,
                Data = products
            };
        }

        
    }
}
