using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.Base;

public class FilterValue(string Value, FilterValueType Type)
{
    public string Value { get; set; } = null!;
    
    public FilterValueType Type { get; set; }
}