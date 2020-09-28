using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SchemaRegistry.Repository.DTO;

namespace SchemaRegistry.Repository
{
    public class SchemaRepository : ISchemaRepository
    {
        private readonly ILogger<SchemaRepository> _logger;

        public SchemaRepository(ILogger<SchemaRepository> logger)
        {
            _logger = logger;
            this.Groups = new SchemaGroupCollection();
        }

        /// <summary>
        /// The schema groups that have been registered
        /// </summary>
        public ISchemaGroupsCollection Groups { get; private set; }
    }
    
    public class SchemaGroupCollection : ISchemaGroupsCollection
    {
        private readonly ConcurrentDictionary<string,SchemaGroupDTO> _store = new ConcurrentDictionary<string, SchemaGroupDTO>();

        /// <summary>
        /// Get all schema groups in namespace.
        /// </summary>
        /// <returns>A list of all the schema group identifiers</returns>
        public Task<string[]> GetAll()
        {
            return Task.FromResult(_store.Keys.ToArray());
        }

        /// <summary>
        /// Add the schema group to the registry
        /// </summary>
        /// <param name="group">The schema group to add</param>
        /// <returns>Nothing</returns>
        /// <exception cref="ConstraintException">Thrown if a schema group with the given ID is already registered</exception>
        public Task AddAsync(SchemaGroupDTO @group)
        {
            if (!_store.TryAdd(@group.ID, @group))
            {
                throw new ConstraintException($"A schema group identified as '{@group.ID}' already exists");
            }
            
            return Task.FromResult(true);
        }

        /// <summary>
        /// Return the schema group that is identified by the given ID
        /// </summary>
        /// <param name="id">The ID of the schema group to return</param>
        /// <returns>The schema group that is identified by the given ID</returns>
        /// <exception cref="KeyNotFoundException">Thrown if there is no group for the given ID</exception>
        public Task<SchemaGroupDTO> FindByIDAsync(string id)
        {
            if (!_store.TryGetValue(id, out var schemaGroup))
            {
                throw new KeyNotFoundException($"There is no schema group identified as '{id}'");
            }
            return Task.FromResult(schemaGroup);
        }

        /// <summary>
        /// Remove the schema group that is identified by the given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Nothing</returns>
        /// <exception cref="KeyNotFoundException">Thrown if there is no group for the given ID</exception>
        public Task RemoveByIDAsync(string id)
        {

            if (!_store.TryRemove(id, out _))
            {
                throw new KeyNotFoundException($"There is no schema group identified as '{id}'");
            }
            return Task.FromResult(true);
        }
    }
}