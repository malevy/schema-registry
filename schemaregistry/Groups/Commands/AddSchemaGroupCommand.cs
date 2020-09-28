using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.Extensions.Logging;
using SchemaRegistry.Repository;
using SchemaRegistry.Repository.DTO;

namespace schemaregistry.Groups.Commands
{
    public static class AddSchemaGroupCommand
    {
        public class Command : IRequest
        {
            public string ID { get; set; }
            public string Description { get; set; }
            public string Format { get; set; }
            public Dictionary<string, string> GroupProperties { get; private set; }

            public void AddAllProperties(Dictionary<string, string> groupProperties)
            {
                Guard.Against.Null(groupProperties, nameof(groupProperties));
                if (null == this.GroupProperties) this.GroupProperties = new Dictionary<string, string>();
                foreach (var property in groupProperties)
                {
                    this.GroupProperties.Add(property.Key, property.Value);
                }
            }
        }
        
        public class Handler : IRequestHandler<Command>
        {
            private readonly ILogger<Handler> _logger;
            private readonly ISchemaRepository _schemaRepository;

            public Handler(ILogger<Handler> logger, ISchemaRepository schemaRepository)
            {
                _logger = logger;
                _schemaRepository = schemaRepository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                Guard.Against.Null(request, nameof(request));
                Guard.Against.NullOrEmpty(request.ID, "group.ID");
                var groupDTO = new SchemaGroupDTO()
                {
                    ID = request.ID,
                    Description = request.Description,
                    Format = request.Format,
                    GroupProperties = request.GroupProperties.ToDictionary(
                        p => p.Key, 
                        p => p.Value)
                };
                
                // TODO - catch exception for duplicate group
                await this._schemaRepository.Groups.AddAsync(groupDTO);

                return Unit.Value;
            }
        }
    }
}