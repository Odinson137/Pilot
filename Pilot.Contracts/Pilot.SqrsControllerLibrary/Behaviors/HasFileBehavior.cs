using MediatR;
using Pilot.SqrsControllerLibrary.Interfaces;

namespace Pilot.SqrsControllerLibrary.Behaviors;

public class HasFileBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IFileUrlService _fileUrlService;

    public HasFileBehavior(IFileUrlService fileUrlService)
    {
        _fileUrlService = fileUrlService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = await next();
        await _fileUrlService.GetUrlAsync(response);

        return response;
    }
}