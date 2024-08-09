using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.Attributes;

[AttributeUsage( AttributeTargets.All )]
public class FromServiceAttribute : Attribute
{
    public ServiceName ServiceName { get; set; } 
    public FromServiceAttribute(ServiceName serviceName)
    {
        ServiceName = serviceName;
    }
}