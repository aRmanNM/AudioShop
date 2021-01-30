using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class VerificationDto
    {
        public string AuthToken { get; set; }
        public string PhoneNumber { get; set; }
        public string UserId { get; set; }
    }
}
