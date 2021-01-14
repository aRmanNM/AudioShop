using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage="وارد کردن نام کاربری ضروریست")]
        public string UserName { get; set; }
        [Required(ErrorMessage="وارد کردن رمز عبور ضروریست")]
        [RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$", ErrorMessage = "رمز عبور به شکل صحیح وارد نشده است - ترکیبی از حروف بزرگ، کوچک، اعداد، و علائم نگارشی به طول حداقل ۶ کاراکتر")]
        public string Password { get; set; }
        public string InvitationCode { get; set; }
        public string Role { get; set; }
        public string PhoneNumber { get; set; }
    }
}
