using Newtonsoft.Json;

namespace API.Dtos
{
    public class PaymentVerificationDto
    {
        [JsonProperty("merchant_id")]
        public string MerchantId { get; set; }
        [JsonProperty("authority")]
        public string Authority { get; set; }
        [JsonProperty("amount")]
        public int Amount { get; set; }
    }
}