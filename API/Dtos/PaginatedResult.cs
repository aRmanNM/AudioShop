using System.Collections.Generic;

namespace API.Dtos
{
    public class PaginatedResult<T>
    {
        public int TotalItems { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}