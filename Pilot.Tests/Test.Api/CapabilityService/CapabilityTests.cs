using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Test.Base.IntegrationBase;
using Xunit.Abstractions;

namespace Test.Api.CapabilityService;

public abstract class CapabilityTests<T, TDto> : BaseApiModelTests<T, TDto>
    where T : BaseModel where TDto : BaseDto
{
    public CapabilityTests(ITestOutputHelper testOutputHelper, ServiceName serviceName,
        ServiceTestConfiguration? apiConfiguration = null,
        IEnumerable<ServiceTestConfiguration>? configurations = null) : base(testOutputHelper, serviceName, apiConfiguration, configurations)
    {
    }
}