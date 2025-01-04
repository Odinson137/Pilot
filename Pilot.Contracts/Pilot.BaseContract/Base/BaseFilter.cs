using System.Text.Json.Serialization;
using Pilot.Contracts.Services;

namespace Pilot.Contracts.Base;

public class BaseFilter(int skip, int take)
{
    public BaseFilter(int? skip = null, int? take = null) : this(skip ?? 0, take ?? 10){}
    
    public BaseFilter(params int[] ids) : this(0, int.MaxValue)
    {
        Ids = ids;
    }
    
    public BaseFilter(ICollection<int> ids) : this(0, int.MaxValue)
    {
        Ids = ids;
    }
    
    public int Skip { get; set; } = skip;
    
    public ICollection<int>? Ids { get; set; }

    public int Take { get; set; } = take;

    public string? SortMember { get; set; }
    
    public string? SortAscending { get; set; }

    public ICollection<(string, string)> QueryParams { get; set; } = [];

    public (string, int)? WhereFilter { get; set; }

    [Newtonsoft.Json.JsonIgnore]
    [JsonIgnore]
    public string Key => GetHashCode().ToString();
    
    public override string ToString() => this.ToJson();
}