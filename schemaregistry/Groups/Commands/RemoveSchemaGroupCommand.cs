using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.Extensions.Logging;
using SchemaRegistry.Repository;

namespace schemaregistry.Groups.Commands
{
    public static class RemoveSchemaGroupCommand
    {
        public class Command : IRequest<Unit>
        {
            public string ID { get; set; }
        }

        public class Handler :  IRequestHandler<Command>
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
                Guard.Against.NullOrEmpty(request.ID, "request.ID");
                await this._schemaRepository.Groups.RemoveByIDAsync(request.ID);
                return Unit.Value;
            }
        }

    }
    
}