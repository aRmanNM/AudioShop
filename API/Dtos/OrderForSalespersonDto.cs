using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using API.Models;

namespace API.Dtos
{
    public class OrderForSalespersonDto
    {
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public decimal SalespersonShareAmount { get; set; }
        public ICollection<string> Courses { get; set; }

        public OrderForSalespersonDto()
        {
            Courses = new Collection<string>();
        }
    }
}