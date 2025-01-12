using System.Text.Json.Serialization;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.Services;

namespace Pilot.Contracts.Base;

public class BaseFilter(int skip, int take)
{
    public BaseFilter() : this(null, null) { }
    
    public BaseFilter(int? skip = null, int? take = null) : this(skip ?? 0, take ?? 10){}

    public BaseFilter(string jsonValue, FilterValueType filterValueType) : this(0, int.MaxValue)
    {
        JsonValue = new FilterValue(jsonValue, filterValueType);
    }
    
    public BaseFilter(params int[] ids) : this(0, int.MaxValue)
    {
        Ids = ids;
    }
    
    public BaseFilter(ICollection<int> ids) : this(0, int.MaxValue)
    {
        Ids = ids;
    }

    public ICollection<int>? Ids { get; set; }
    
    public int Skip { get; set; } = skip;

    public int Take { get; set; } = take;

    public string? SortMember { get; set; }
    
    public string? SortAscending { get; set; }

    public ICollection<(string, string)> QueryParams { get; set; } = [];

    public (string, int)? WhereFilter { get; set; }

    public FilterValue? JsonValue { get; set; }
    
    public override string ToString() => this.ToJson();
}