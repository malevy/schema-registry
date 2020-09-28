using System.Collections.Generic;
using System.Threading.Tasks;
using SchemaRegistry.Repository.DTO;

namespace SchemaRegistry.Repository
{
    public interface ISchemaRepository
    {
        /// <summary>
        /// The schema groups that have been registered
        /// </summary>
        ISchemaGroupsCollection Groups { get; }
    }

    public interface ISchemaGroupsCollection
    {
        /// <summary>
        /// Get all schema groups in namespace.
        /// </summary>
        /// <returns>A list of all the schema group identifiers</returns>
        Task<string[]> GetAll();
        
        /// <summary>
        /// Add the schema group to the registry
        /// </summary>
        /// <param name="group">The schema group to add</param>
        /// <returns>Nothing</returns>
        /// <exception cref="ConstraintException">Thrown if a schema group with the given ID is already registered</exception>
        Task AddAsync(SchemaGroupDTO group);

        /// <summary>
        /// Return the schema group that is identified by the given ID
        /// </summary>
        /// <param name="ID">The ID of the schema group to return</param>
        /// <returns>The schema group that is identified by the given ID</returns>
        /// <exception cref="KeyNotFoundException">Thrown if there is no group for the given ID</exception>
        Task<SchemaGroupDTO> FindByIDAsync(string ID);
        
        /// <summary>
        /// Remove the schema group that is identified by the given ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>Nothing</returns>
        /// <exception cref="KeyNotFoundException">Thrown if there is no group for the given ID</exception>
        Task RemoveByIDAsync(string ID);
    }
}