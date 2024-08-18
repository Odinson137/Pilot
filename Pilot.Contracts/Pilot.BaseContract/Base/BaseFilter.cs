namespace Pilot.Contracts.Base;

public struct BaseFilter(int skip, int take)
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
}