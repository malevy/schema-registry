using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SchemaRegistry.Repository.DTO
{
    public class SchemaGroupDTO
    {
        private Dictionary<string, object> _schemas;

        [JsonPropertyName("id")]
        public string ID { get; set; }
        public string Description { get; set; }
        public string Format { get; set; }
        public string CreatedTimeUTC { get; set; }
        public string UpdatedTimeUTC { get; set; }
        public Dictionary<string, string> GroupProperties { get; set; }

        /// <summary>
        /// the list of schemas within this group
        /// </summary>
        public Dictionary<string, object> Schemas
        {
            get
            {
                if (null == _schemas) _schemas = new Dictionary<string, object>();
                return _schemas;
            }
            set => _schemas = value;
        }
    }
}