using Newtonsoft.Json;

namespace API.Dtos.ZarinPal
{
    public class VerificationResult : RequestResult
    {
        // USED FOR SANDBOX
		[JsonProperty("RefID")]
		public long RefId { get; set; }
    }
}
