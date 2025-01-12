using MediatR;
using Pilot.Contracts.Base;
using Pilot.SqrsControllerLibrary.Commands;
using Pilot.SqrsControllerLibrary.Interfaces;

namespace Pilot.Api.Behaviors;

public class AddFileBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<BaseDto>
{
    private readonly IFileService _fileUrlService;

    public AddFileBehavior(IFileService fileUrlService)
    {
        _fileUrlService = fileUrlService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        await _fileUrlService.AddFileAsync(request, cancellationToken);
        var response = await next();
        return response;
    }
}