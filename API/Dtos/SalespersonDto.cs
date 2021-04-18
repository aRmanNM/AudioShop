using API.Models;

namespace API.Dtos
{
    public class SalespersonDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public short Age { get; set; }
        public Gender Gender { get; set; }
        public int SalePercentageOfSalesperson { get; set; }
        public int DiscountPercentageOfSalesperson { get; set; }
        public decimal CurrentSalesOfSalesperson { get; set; }
        public decimal TotalSalesOfSalesperson { get; set; }
        public string CouponCode { get; set; }
        public SalespersonCredential SalespersonCredential { get; set; }
        public bool CredentialAccepted { get; set; } = false;
    }
}