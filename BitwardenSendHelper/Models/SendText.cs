using Newtonsoft.Json;

namespace BitwardenSendHelper.Models
{ 
    public class SendText
    {
        [JsonProperty("text")] 
        public string Text { get; set; }
        
        [JsonProperty("hidden")] 
        public bool IsHidden { get; set; }
    }
}

