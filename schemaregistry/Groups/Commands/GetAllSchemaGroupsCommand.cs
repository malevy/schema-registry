using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.Extensions.Logging;
using SchemaRegistry.Repository;
using SchemaRegistry.Repository.DTO;

namespace schemaregistry.Groups.Commands
{
    public static class GetAllSchemaGroupsCommand
    {
        public class Command : IRequest<string[]>
        {
        }
        
        public class Handler :  IRequestHandler<Command, string[]>
        {
            private readonly ILogger<Handler> _logger;
            private readonly ISchemaRepository _schemaRepository;

            public Handler(ILogger<Handler> logger, ISchemaRepository schemaRepository)
            {
                _logger = logger;
                _schemaRepository = schemaRepository;
            }

            public Task<string[]> Handle(Command request, CancellationToken cancellationToken)
            {
                return this._schemaRepository.Groups.GetAll();
            }
        }

    }
    
}