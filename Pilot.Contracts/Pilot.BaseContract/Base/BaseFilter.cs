using System.Linq.Expressions;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.Base;

public class BaseFilter(int skip, int take)
{
    public BaseFilter() : this(null, null)
    {
    }

    public BaseFilter(int? skip = null, int? take = null) : this(skip ?? 0, take ?? 10)
    {
    }

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
    public WhereFilter()
    {
    }

    public WhereFilter(params (string, object)[] queryParams)
    {
        AddToList(queryParams);
    }

    private void AddToList((string, object)[] queryParams)
    {
        foreach (var param in queryParams)
            AddToList(param);
    }

    private void AddToList((string, object) queryParams)
    {
        List.Add(
            new ValueTuple<string, object, Type>(queryParams.Item1, queryParams.Item2, queryParams.Item2.GetType()));
    }

    public void Init<TValue, TViewModel>((Expression<Func<TViewModel, TValue>> predicate, TValue value) valueTuple)
        where TValue : IConvertible
    {
        var fieldsName = valueTuple.predicate.Body.ToString().Split(".").Skip(1).ToList();
        if (valueTuple.value is null) throw new ArgumentNullException("Ты чёт попутал");

        var names = string.Join(".", fieldsName);
        AddToList((names, valueTuple.value));
    }

    public ICollection<(string, object, Type)> List { get; } = [];
}