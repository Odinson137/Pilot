using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Identity.Interfaces;
using Pilot.Identity.Models;
using Pilot.SqrsControllerLibrary.Commands;
using Pilot.SqrsControllerLibrary.Handlers;

namespace Pilot.Identity.Handlers;

public class UserCommandHandler(IUser repository, IMapper mapper, IBaseValidatorService validateService)
    : ModelCommandHandler<User, UserDto>(repository, mapper, validateService)
{
    public override async Task<BaseModel> Handle(
        UpdateEntityCommand<UserDto> request,
        CancellationToken cancellationToken)
    {
        var model = mapper.Map<User>(request.Value);

        var existingModel = await repository.GetByIdAsync(request.Value.Id, cancellationToken);
        if (existingModel == null)
            throw new Exception("Entity not found");

        existingModel.AvatarImage = model.AvatarImage;
        existingModel.Name = model.Name;
        existingModel.LastName = model.LastName;
        existingModel.Birthday = model.Birthday;
        existingModel.City = model.City;
        existingModel.Country = model.Country;
        existingModel.Description = model.Description;
        existingModel.Email = model.Email;
        existingModel.Gender = model.Gender;
        existingModel.UserName = model.UserName;

        existingModel.ChangeAt = DateTime.Now;

        return model;
    }
}
