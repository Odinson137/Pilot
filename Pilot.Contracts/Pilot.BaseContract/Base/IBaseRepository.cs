﻿using Microsoft.EntityFrameworkCore;

namespace Pilot.Contracts.Base;

public interface IBaseRepository<T> : IBaseReadRepository<T> where T : BaseModel
{
    public DbContext Context { get; }
    public Task<T> AddValueToContextAsync(T value, CancellationToken token = default);

    public Task SaveAsync(CancellationToken token = default);

    public Task<int> FastDeleteAsync(int modelId, CancellationToken token = default);
    
    public void Delete(T model);
}