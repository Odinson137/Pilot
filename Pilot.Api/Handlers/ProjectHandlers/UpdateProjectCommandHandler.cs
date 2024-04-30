using MassTransit;
using MediatR;
using Pilot.Api.DTO;
using Pilot.Contracts.RabbitMqMessages.Project;

namespace Pilot.Api.Handlers.ProjectHandlers;

public record UpdateProjectCommand(UpdateProjectDto UpdateProject) 
    : IRequest;

public class UpdateProjectCommandHandler
    : IRequestHandler<UpdateProjectCommand>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public UpdateProjectCommandHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        await _publishEndpoint.Publish(
            new ProjectUpdated
            {
                Id = request.UpdateProject.Id,
                CompanyId = request.UpdateProject.CompanyId,
                Name = request.UpdateProject.Name,
                Description = request.UpdateProject.Description,
                ProjectStatus = request.UpdateProject.ProjectStatus,
            }, cancellationToken);
    }
}