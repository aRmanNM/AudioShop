namespace API.Models
{
    public class SalespersonCredential
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string IdCardNumber { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAccountShebaNumber { get; set; }
        public string BankCardNumber { get; set; }
        public string BankCardName { get; set; }
        public string BankName { get; set; }
        public string Phone { get; set; }
        public Photo IdCardPhoto { get; set; }
        public Photo BankCardPhoto { get; set; }
        public string Message { get; set; }
    }
}