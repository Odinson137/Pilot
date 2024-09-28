using Pilot.Contracts.Base;

namespace Pilot.Worker.Interface;

public interface ICompany : IBaseRepository<Models.Company>
{
    public Task<bool> CheckCompanyTitleExistAsync(string title);
}