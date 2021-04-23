using API.Dtos;
using API.Dtos.ZarinPal;
using API.Helpers;
using API.Interfaces;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace API.Services
{
    public class ZarinPalService : IZarinPalService
    {
        public async Task<RequestResult> Request(PaymentRequestDto requestDto, PaymentMode mode)
        {
            var baseUrl = mode == PaymentMode.zarinpal ? 
                "https://api.zarinpal.com/pg" : "https://sandbox.zarinpal.com/pg";

            var serialized = JsonConvert.SerializeObject(requestDto);
            var content = new StringContent(serialized, Encoding.UTF8, "application/json");

            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{baseUrl}/v4/payment/request.json", content);
            var result = await response.Content.ReadAsStringAsync();            
            return JsonConvert.DeserializeObject<RequestResult>(result);
        }

        public async Task<VerificationResult> Verification(PaymentVerificationDto verificationDto, PaymentMode mode)
        {
            var baseUrl = mode == PaymentMode.zarinpal ?
                "https://api.zarinpal.com/pg" : "https://sandbox.zarinpal.com/pg";

            var serialized = JsonConvert.SerializeObject(verificationDto);
            var content = new StringContent(serialized, Encoding.UTF8, "application/json");

            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{baseUrl}/v4/payment/verify.json", content);
            var result = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<VerificationResult>(result);
        }
    }
}
