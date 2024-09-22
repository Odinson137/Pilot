using System.Text.Json.Serialization;
using Pilot.Contracts.Services;

namespace Pilot.Contracts.Base;

public class BaseFilter(int skip, int take)
{
    public BaseFilter() : this(0, 10) { }

    public BaseFilter(params int[] ids) : this(0, int.MaxValue)
    {
        Ids = ids;
    }
    
    public int Skip { get; set; } = skip;
    
    public ICollection<int>? Ids { get; set; }

    public int Take { get; set; } = take;

    public string? SortMember { get; set; }
    
    public string? SortAscending { get; set; }

    [Newtonsoft.Json.JsonIgnore]
    [JsonIgnore]
    public string Key => GetHashCode().ToString();
    
    public override string ToString() => this.ToJson();
}