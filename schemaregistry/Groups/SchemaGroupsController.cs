using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using schemaregistry.Groups.Commands;
using schemaregistry.Groups.Messages;

namespace schemaregistry.Groups
{
    [ApiController]
    [Route("[controller]")]
    public class SchemaGroupsController : ControllerBase
    {
        private readonly ILogger<SchemaGroupsController> _logger;
        private readonly IMediator _mediator;

        public SchemaGroupsController(ILogger<SchemaGroupsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        
        
        /// <summary>
        /// Retrieve the list of Schema Groups
        /// </summary>
        /// <returns>
        /// 200 OK - List of registered schema groups
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await this._mediator.Send(new GetAllSchemaGroupsCommand.Command());
            return this.Ok(result);
        }

        /// <summary>
        /// Retrieve a specific schema-group
        /// </summary>
        /// <param name="groupId">The ID of the group to retreive</param>
        /// <returns>
        /// 200 OK - The schema-group associated with the given ID
        /// 404 NOT FOUND - no schema-group exists with the given ID
        /// </returns>
        [HttpGet("{groupId}", Name = nameof(GetGroup))] // the Name property is required for the CreateAtRoute() to work
        public async Task<IActionResult> GetGroup([FromRoute] string groupId)
        {
            var getGroupCommand = new GetSchemaGroupCommand.Command()
            {
                ID = groupId
            };

            try
            {
                var result = await this._mediator.Send(getGroupCommand);
                return this.Ok(SchemaGroupResponseMessage.From(result));
            }
            catch (KeyNotFoundException)
            {
                return this.NotFound($"no schema group with ID '{groupId}'");
            }


        }

        /// <summary>
        /// Create schema group with specified format in registry namespace.
        /// </summary>
        /// <param name="groupId">The ID of the group to create</param>
        /// <param name="requestMessage">Attributes of the group being created</param>
        /// <returns>
        /// 201 Created - The Location header will contain the URL of the newly created schema-group
        /// 409 Conflict - A schema-group with the given ID already exists
        /// </returns>
        [HttpPut("{groupId}")]
        public async Task<IActionResult> CreateGroup([FromRoute] string groupId, [FromBody] SchemaGroupRequestMessage requestMessage)
        {
            var request = requestMessage ?? new SchemaGroupRequestMessage();
            
            var addCommand = new AddSchemaGroupCommand.Command()
            {
                Description = request.Description,
                Format = request.Format,
                ID = groupId
            };
            addCommand.AddAllProperties(request.GroupProperties);

            try
            {
                await this._mediator.Send(addCommand);
                return this.CreatedAtRoute(nameof(GetGroup), new {groupId = groupId}, null);
            }
            catch (ConstraintException)
            {
                return this.Conflict($"a schema group with the ID '{groupId}' is already registered.");
            }
        }

        /// <summary>
        /// Delete schema group in schema registry namespace
        /// </summary>
        /// <param name="groupId">The ID of the group to remove</param>
        /// <returns>
        /// 204 NO CONTENT - the schema-group was successfully removed
        /// 404 NOT FOUND - no schema-group exists with the given ID
        /// </returns>
        [HttpDelete("{groupId}")]
        public async Task<IActionResult> DeleteGroup([FromRoute] string groupId)
        {
            var command = new RemoveSchemaGroupCommand.Command()
            {
                ID = groupId
            };

            try
            {
                await this._mediator.Send(command);
                return this.NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"No schema group with key '{groupId}'");
            }
        }
        
    }
}