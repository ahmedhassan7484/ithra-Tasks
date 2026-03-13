using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ProductFilterDto
    {
        public string? Search { get; set; }

        public string? Category { get; set; }

        public string? SortBy { get; set; }

        public string? SortDirection { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }

}
