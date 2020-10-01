using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using SchemaRegistry.Repository;

namespace SchemaRegistry.Schemas.Commands
{
    public static class DeleteAllSchemasByGroupCommand
    {
        public class Command : IRequest<Unit>
        {
            public string GroupId { get; set; }
        }
        
        public class Handler : IRequestHandler<Command>
        {
            private readonly ISchemaRepository _repository;

            public Handler(ISchemaRepository repository)
            {
                _repository = repository;
            }
            
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                Guard.Against.NullOrEmpty(request.GroupId, "groupId");
                var schemaGroup = await _repository.Groups.FindByIDAsync(request.GroupId);
                schemaGroup.Schemas.Clear();
                
                return Unit.Value;
            }
        }
    }
}