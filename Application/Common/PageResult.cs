using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public class PageResult<T>
    {
      
            public int Page { get; set; }

            public int PageSize { get; set; }

            public int TotalCount { get; set; }

            public IEnumerable<T> Data { get; set; }
        
    }
}
