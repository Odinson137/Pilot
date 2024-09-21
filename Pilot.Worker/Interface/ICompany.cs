using Pilot.Contracts.Base;
using Pilot.Worker.Models;

namespace Pilot.Worker.Interface;

public interface ICompany : IBaseRepository<Models.Company>
{
    public Task<bool> CheckCompanyTitleExistAsync(string title);
}