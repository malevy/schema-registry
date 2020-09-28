using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.Extensions.Logging;
using SchemaRegistry.Repository;
using SchemaRegistry.Repository.DTO;

namespace schemaregistry.Groups.Commands
{
    public static class GetSchemaGroupCommand
    {
        public class Command : IRequest<SchemaGroupDTO>
        {
            public string ID { get; set; }
        }
        
        public class Handler :  IRequestHandler<Command, SchemaGroupDTO>
        {
            private readonly ILogger<Handler> _logger;
            private readonly ISchemaRepository _schemaRepository;

            public Handler(ILogger<Handler> logger, ISchemaRepository schemaRepository)
            {
                _logger = logger;
                _schemaRepository = schemaRepository;
            }

            public Task<SchemaGroupDTO> Handle(Command request, CancellationToken cancellationToken)
            {
                Guard.Against.NullOrEmpty(request.ID, "request.ID");
                return this._schemaRepository.Groups.FindByIDAsync(request.ID);
            }
        }

    }
    
}