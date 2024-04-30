using MassTransit;
using MediatR;
using Pilot.Api.DTO;
using Pilot.Contracts.RabbitMqMessages.Project;

namespace Pilot.Api.Handlers.ProjectHandlers;

public record CreateProjectCommand(CreateProjectDto CreateProject) 
    : IRequest;

public class CreateProjectQueryHandler
    : IRequestHandler<CreateProjectCommand>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateProjectQueryHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        await _publishEndpoint.Publish(
            new ProjectCreated
            {
                CompanyId = request.CreateProject.CompanyId,
                Name = request.CreateProject.Name,
                Description = request.CreateProject.Description,
                ProjectStatus = request.CreateProject.ProjectStatus,
            }, cancellationToken);
    }
}