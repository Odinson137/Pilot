using Pilot.Contracts.Data.Enums;

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

    public WhereFilter? WhereFilter { get; set; }

    public FilterValue? JsonValue { get; set; }
}

public class WhereFilter
{
    public WhereFilter() { }

    public WhereFilter(params (string, object)[] queryParams)
    {
        foreach (var param in queryParams)
            List.Add(new ValueTuple<string, object, Type>(param.Item1, param.Item2, param.Item2.GetType()));
    }

    public ICollection<(string, object, Type)> List { get; set; } = [];
}