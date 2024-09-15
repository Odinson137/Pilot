using System.ComponentModel;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Capability.Data.Enums;

public enum CapabilityModelType
{
    [Description(nameof(CompanyDto))] Company = ModelType.Company,
    
    [Description(nameof(UserDto))] User = ModelType.User,
}