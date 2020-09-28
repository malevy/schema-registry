using System.Collections.Generic;
using System.Security.Cryptography;
using Ardalis.GuardClauses;
using SchemaRegistry.Repository.DTO;

namespace schemaregistry.Groups.Messages
{

    public class SchemaGroupRequestMessage : SchemaGroupBase
    {
        
    }

    public class SchemaGroupResponseMessage : SchemaGroupBase
    {
        /// <summary>
        /// Identifies the schema group
        /// </summary>
        /// <remarks>
        /// Constraints:
        /// - required
        /// - MUST be a non-empty string
        /// - MUST conform with RFC3986/3.3 syntax
        /// - MUST be unique within the registry
        /// </remarks>
        public string ID { get; set; }

        /// <summary>
        /// Instant when the schema group was added to the registry
        /// </summary>
        /// <remarks>
        /// Constraints:
        /// - optional
        /// - assigned by the server
        /// </remarks>
        public string CreatedTimeUTC { get; set; }

        /// <summary>
        /// Instant when the schema group was last updated
        /// </summary>
        /// <remarks>
        /// Constraints:
        /// - optional
        /// - assigned by the server
        /// </remarks>
        public string UpdatedTimeUTC { get; set; }

        public static SchemaGroupResponseMessage From(SchemaGroupDTO dto)
        {
            Guard.Against.Null(dto, nameof(dto));
            var response = new SchemaGroupResponseMessage()
            {
                ID = dto.ID,
                CreatedTimeUTC = dto.CreatedTimeUTC,
                UpdatedTimeUTC = dto.UpdatedTimeUTC,
                Description = dto.Description,
                Format = dto.Format,
                GroupProperties = new Dictionary<string, string>(dto.GroupProperties)
            };
            return response;
        }
    }
    
    public abstract class SchemaGroupBase
    {
        private Dictionary<string, string> _groupProperties;

        /// <summary>
        /// Explains the purpose of the schema group
        /// </summary>
        /// <remarks>
        /// Constraints:
        /// - optional
        /// </remarks>
        public string Description { get; set; }
        
        /// <summary>
        /// Defines the schema format managed by this schema group.
        /// If the format is omitted from the schema group, the format MUST be specified at the schema level
        /// </summary>
        /// <remarks>
        /// Constraints:
        /// - optional
        /// - MUST be non-empty string, if present
        /// - MUST NOT be modified if at least one schema exists in the group
        /// </remarks>
        public string Format { get; set; }

        /// <summary>
        /// Additional properties of the schema group
        /// </summary>
        public Dictionary<string, string> GroupProperties
        {
            get => _groupProperties ??= new Dictionary<string, string>();
            set => _groupProperties = value;
        }
    }
}