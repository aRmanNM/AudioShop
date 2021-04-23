using Newtonsoft.Json;

namespace API.Dtos
{
    public class PaymentRequestDto
    {
        [JsonProperty("merchant_id")]
        public string MerchantId { get; set; }        
        [JsonProperty("amount")]
        public int Amount { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("mobile")]
        public string Mobile { get; set; }
        [JsonProperty("callback_url")]
        public string CallbackUrl { get; set; }
    }
}