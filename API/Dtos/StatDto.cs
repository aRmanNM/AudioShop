using System;

namespace API.Dtos
{
    public class StatDto
    {
        public int Id { get; set; }
        public string TitleEn { get; set; }
        public string TitleFa { get; set; }
        public int Counter { get; set; }
        public DateTime DateOfStat { get; set; }
    }
}