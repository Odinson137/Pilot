using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.Base;

public class FilterValue
{
    public FilterValue(string value, FilterValueType type)
    {
        Value = value;
        Type = type;
    }
    public string Value { get; set; } = null!;
    
    public FilterValueType Type { get; set; }
}