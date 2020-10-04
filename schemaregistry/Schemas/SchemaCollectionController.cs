using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchemaRegistry.Filters;
using SchemaRegistry.Schemas.Commands;

namespace SchemaRegistry.Schemas
{
    [ApiController]
    [ServiceFilter(typeof(LoggingActionFilter))]
    [Route("schemagroups/{groupId}/schemas")]
    public class SchemaController : ControllerBase
    {
        private readonly ILogger<SchemaController> _logger;
        private readonly IMediator _mediator;

        public SchemaController(ILogger<SchemaController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        
        /// <summary>
        /// Returns schema by group id.
        /// </summary>
        /// <param name="groupId">schema group</param>
        /// <returns>
        /// 200 OK - the list of schemas for the given group
        /// 404 NOT FOUND - group not found
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetSchemasByGroup(string groupId)
        {
            var command = new GetSchemasForGroupCommand.Command()
            {
                GroupId = groupId
            };

            try
            {
                var schemaList = await this._mediator.Send(command);
                return this.Ok(schemaList);
            }
            catch (KeyNotFoundException)
            {
                return this.NotFound($"no schema group with ID '{groupId}'");
            }            
        }

        /// <summary>
        /// Deletes all schemas under specified group id
        /// </summary>
        /// <param name="groupId">schema group</param>
        /// <returns>
        /// 204 NO CONTENT - schemas successfully removed
        /// 404 NOT FOUND - group not found
        /// </returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteAllSchemasByGroup(string groupId)
        {
            var command = new DeleteAllSchemasByGroupCommand.Command()
            {
                GroupId = groupId
            };
            try
            {
                var result = await _mediator.Send(command);
                return this.NoContent();
            }
            catch (KeyNotFoundException)
            {
                return this.NotFound($"no schema group with ID '{groupId}'");
            }            
        }

    }
}