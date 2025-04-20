using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Commands;
using Pilot.SqrsControllerLibrary.Handlers;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Handlers;

public class CompanyCommandHandler(ICompany repository, IMapper mapper, IBaseValidatorService validateService)
    : ModelCommandHandler<Company, CompanyDto>(repository, mapper, validateService)
{
    private readonly IMapper _mapper = mapper;
    private readonly IBaseValidatorService _validateService = validateService;

    public override async Task<BaseModel> Handle(
        CreateEntityCommand<CompanyDto> request,
        CancellationToken cancellationToken)
    {
        await _validateService.ValidateAsync<Company, CompanyDto>(request.Value);

        var model = _mapper.Map<Company>(request.Value);

        await _validateService.ChangeEntityTrackerAsync(model);
        var firstEmployee = new CompanyUser
        {
            Id = request.UserId,
            Company = model
        };
        model.CompanyUsers.Add(firstEmployee);
        model.CreatedBy = firstEmployee;

        await repository.AddValueToContextAsync(model, cancellationToken);
        return model;
    }
}