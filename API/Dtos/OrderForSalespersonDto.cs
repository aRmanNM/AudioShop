using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace API.Dtos
{
    public class OrderForSalespersonDto
    {
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public decimal SalespersonShareAmount { get; set; }
        public ICollection<EpisodeDto> Episodes { get; set; }

        public OrderForSalespersonDto()
        {
            Episodes = new Collection<EpisodeDto>();
        }
    }
}