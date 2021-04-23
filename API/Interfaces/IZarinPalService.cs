using System.Threading.Tasks;
using API.Dtos;
using API.Dtos.ZarinPal;
using API.Helpers;

namespace API.Interfaces
{
    public interface IZarinPalService
    {
         Task<RequestResult> Request(PaymentRequestDto requestDto, PaymentMode mode);
         Task<VerificationResult> Verification(PaymentVerificationDto data, PaymentMode mode);
    }
}