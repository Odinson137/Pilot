﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;
using Pilot.SqrsControllerLibrary.Commands;

namespace Pilot.SqrsControllerLibrary.Handlers;

public class ModelCommandHandler<T, TDto>(
    IBaseRepository<T> repository,
    IMapper mapper,
    IBaseValidatorService validateService) :
        IRequestHandler<CreateEntityCommand<TDto>, BaseModel>,
        IRequestHandler<UpdateEntityCommand<TDto>, BaseModel>,
        IRequestHandler<DeleteEntityCommand<TDto>, BaseModel>
    where T : BaseModel, new()
    where TDto : BaseDto
{
    public virtual async Task<BaseModel> Handle(
        CreateEntityCommand<TDto> request,
        CancellationToken cancellationToken)
    {
        await validateService.ValidateAsync<T, TDto>(request.Value);

        var model = mapper.Map<T>(request.Value);

        await validateService.ChangeEntityTrackerAsync(model);

        await repository.AddValueToContextAsync(model, cancellationToken);
        return model;
    }

    public virtual async Task<BaseModel> Handle(
        UpdateEntityCommand<TDto> request,
        CancellationToken cancellationToken)
    {
        var model = mapper.Map<T>(request.Value);

        var existingModel = await repository.GetByIdAsync(request.Value.Id, cancellationToken);
        if (existingModel == null)
            throw new Exception("Entity not found");

        var entityEntry = repository.Context.Entry(existingModel);
        entityEntry.CurrentValues.SetValues(model);

        existingModel.ChangeAt = DateTime.Now;

        entityEntry.State = EntityState.Modified;
        return model;
    }

    public virtual async Task<BaseModel> Handle(DeleteEntityCommand<TDto> request, CancellationToken cancellationToken)
    {
        var model = await validateService.DeleteValidateAsync<T>(request.Value, cancellationToken);
        repository.Delete(model);
        return model;
    }
}