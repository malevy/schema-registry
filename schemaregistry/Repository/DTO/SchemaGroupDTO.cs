using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SchemaRegistry.Repository.DTO
{
    public class SchemaGroupDTO
    {
        [JsonPropertyName("id")]
        public string ID { get; set; }
        public string Description { get; set; }
        public string Format { get; set; }
        public string CreatedTimeUTC { get; set; }
        public string UpdatedTimeUTC { get; set; }
        public Dictionary<string, string> GroupProperties { get; set; }
    }
}