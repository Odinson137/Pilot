using MediatR;
using Pilot.SqrsControllerLibrary.Interfaces;

namespace Pilot.Api.Behaviors;

public class HasFileBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IFileService _fileUrlService;

    public HasFileBehavior(IFileService fileUrlService)
    {
        _fileUrlService = fileUrlService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = await next();
     // пока отключил
        // await _fileUrlService.GetUrlAsync(response);
        return response;
    }
}