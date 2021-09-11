using API.Dtos;
using API.Dtos.ZarinPal;
using API.Helpers;
using API.Interfaces;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace API.Services
{
    public class ZarinPalService : IZarinPalService
    {
        private readonly HttpClient _httpClient;
        public ZarinPalService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<RequestResult> Request(PaymentRequestDto requestDto, PaymentMode mode)
        {
            var baseUrl = mode == PaymentMode.zarinpal ?
                "https://api.zarinpal.com/pg/v4/payment/request.json" : "https://sandbox.zarinpal.com/pg/rest/WebGate/PaymentRequest.json";

            string serialized;
            if (mode == PaymentMode.zarinpal)
            {
                serialized = JsonConvert.SerializeObject(new
                {
                    merchant_id = requestDto.MerchantId,
                    amount = requestDto.Amount,
                    callback_url = requestDto.CallbackUrl,
                    description = requestDto.Description,
                    mobile = requestDto.Mobile ?? "",
                    email = requestDto.Email ?? ""
                });
            }
            else
            {
                serialized = JsonConvert.SerializeObject(new
                {
                    MerchantID = requestDto.MerchantId,
                    Amount = requestDto.Amount,
                    CallbackURL = requestDto.CallbackUrl,
                    Description = requestDto.Description
                });
            }

            var content = new StringContent(serialized, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(baseUrl, content);
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RequestResult>(result);
        }

        public async Task<VerificationResult> Verification(PaymentVerificationDto verificationDto, PaymentMode mode)
        {
            var baseUrl = mode == PaymentMode.zarinpal ?
                "https://api.zarinpal.com/pg/v4/payment/verify.json" : "https://sandbox.zarinpal.com/pg/rest/WebGate/PaymentVerification.json";

            var serialized = JsonConvert.SerializeObject(verificationDto);
            var content = new StringContent(serialized, Encoding.UTF8, "application/json");

            // using var httpClient = new HttpClient();
            var response = await _httpClient.PostAsync($"{baseUrl}/v4/payment/verify.json", content);
            var result = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<VerificationResult>(result);
        }
    }
}
