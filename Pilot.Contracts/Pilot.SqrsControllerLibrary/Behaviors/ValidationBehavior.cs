// using System.ComponentModel.DataAnnotations;
// using MediatR;
// using Microsoft.EntityFrameworkCore;
// using Pilot.Contracts.Base;
// using Pilot.SqrsControllerLibrary.Interfaces;
//
// namespace Pilot.SqrsControllerLibrary.Behaviors;
//
// public class ValidationBehavior<TRequest, TResponse> 
//     : IPipelineBehavior<TRequest, TResponse>
//     where TRequest : IEntityCommand<TResponse>
//     where TResponse : BaseModel
// {
//     private readonly DbContext _context;
//
//     public ValidationBehavior(DbContext context)
//     {
//         _context = context;
//     }
//
//     public async Task<TResponse> Handle(
//         TRequest request, 
//         RequestHandlerDelegate<TResponse> next,
//         CancellationToken cancellationToken)
//     {
//         var isValidate = await _context.Set<TResponse>().Validate(model);
//
//         var validationResult = await _validator.ValidateAsync<TRequest, TResponse>(request.ValueDto);
//
//         if (!validationResult.IsValid)
//         {
//             throw new ValidationException(validationResult.Errors);
//         }
//
//         var model = await next();
//         return model.;
//     }
// }