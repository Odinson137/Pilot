using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Identity.Interfaces;
using Pilot.Identity.Models;
using Pilot.SqrsControllerLibrary.Handlers;

namespace Pilot.Identity.Handlers;

public class UserCommandHandler(IUser repository, IMapper mapper, IBaseValidatorService validateService)
    : ModelCommandHandler<User, UserDto>(repository, mapper, validateService)
{
    // public override async Task<BaseModel> Handle(
    //     CreateEntityCommand<CompanyDto> request,
    //     CancellationToken cancellationToken)
    // {
    //     await validateService.ValidateAsync<User, UserDto>(request.Value);
    //
    //     var model = mapper.Map<User>(request.Value);
    //
    //     await validateService.ChangeEntityTrackerAsync(model);
    //     var firstEmployee = new User
    //     {
    //         Id = request.UserId,
    //         Company = model
    //     };
    //     model.CompanyUsers.Add(firstEmployee);
    //     await repository.AddValueToContextAsync(model, cancellationToken);
    //     return model;
    // }
}