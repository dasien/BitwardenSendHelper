using Newtonsoft.Json;

namespace BitwardenSendHelper.Models
{
    public class SendResponse
    {
    
        [JsonProperty("object")] 
        public string ObjectType { get; set; }
        
        [JsonProperty("id")] 
        public string Id { get; set; }

        [JsonProperty("accessId")] 
        public string AccessId { get; set; }
        
        [JsonProperty("accessUrl")] 
        public string AccessUrl { get; set; }

        [JsonProperty("name")] 
        public string Name { get; set; }

        [JsonProperty("notes")] 
        public string Notes { get; set; }
        
        [JsonProperty("key")]
        public string Key { get; set; }
        
        [JsonProperty("type")] 
        public string SendType { get; set; }

        [JsonProperty("text")] 
        public SendText? Text { get; set; }
        
        [JsonProperty("file")] 
        public SendFile? File { get; set; }
        
        [JsonProperty("maxAccessCount")] 
        public int? MaxAccessCount { get; set; }

        [JsonProperty("accessCount")] 
        public int? AccessCount { get; set; }

        [JsonProperty("revisionDate")] 
        public DateTime RevisionDate { get; set; }

        [JsonProperty("deletionDate")] 
        public DateTime DeletionDate { get; set; }

        [JsonProperty("expirationDate")] 
        public DateTime? ExpirationDate { get; set; }

        [JsonProperty("passwordSet")] 
        public bool IsPasswordSet { get; set; }

        [JsonProperty("disabled")] 
        public bool IsDisabled { get; set; }
        
        [JsonProperty("hideEmail")] 
        public bool IsEmailHidden { get; set; }

    }    
}
