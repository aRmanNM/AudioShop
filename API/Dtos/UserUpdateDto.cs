using System.ComponentModel.DataAnnotations;
using API.Models;

namespace API.Dtos
{
    public class UserUpdateDto
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public short Age { get; set; }
        public Gender Gender { get; set; }
        public bool Employed { get; set; }
    }
}
