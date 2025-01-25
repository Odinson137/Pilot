using MediatR;
using Pilot.Contracts.Base;
using Pilot.SqrsControllerLibrary.Interfaces;

namespace Pilot.Api.Behaviors;

public class ChangeFileBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest
{
    private readonly IFileService _fileUrlService;

    public ChangeFileBehavior(IFileService fileUrlService)
    {
        _fileUrlService = fileUrlService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // if (request is ICommand<BaseDto> command)
        //     await _fileUrlService.ChangeFileAsync(command, cancellationToken);

        var response = await next();
        return response;
    }
}