using Pilot.Contracts.Base;
using Pilot.Contracts.Models;

namespace Pilot.Receiver.Interface;

public interface ICompany : IBaseRepository<Company>
{
    public Task<bool> CheckCompanyTitleExistAsync(string title);
}