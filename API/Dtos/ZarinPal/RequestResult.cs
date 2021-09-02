using Newtonsoft.Json;
using System;

namespace API.Dtos.ZarinPal
{
    public class RequestResult
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
        [JsonProperty("errors")]
        public string[] Errors { get; set; }


        // USED IN SANDBOX
        [JsonProperty("Authority")]
        public string Authority { get; set; }
        [JsonProperty("Status")]
        public string Status { get; set; }
    }

    public class Data
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("authority")]
        public string Authority { get; set; }
        [JsonProperty("ref_id")]
        public long RefId { get; set; }
    }
}
