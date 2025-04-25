using System.Linq.Expressions;
using Org.BouncyCastle.Asn1.X509.Qualified;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.Exception.ApiExceptions;

namespace Pilot.Contracts.Base;

public class BaseFilter(int skip, int take)
{
    public BaseFilter() : this(null, null)
    {
    }

    public BaseFilter(int? skip = null, int? take = null) :
        this(skip ?? 0, take ?? int.MaxValue) // потом поменять на дефолтное 5
    {
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

    public string? SelectQuery { get; set; }

    public WhereFilter? WhereFilter { get; set; }

    public string GetKey<TDto>()
    {
        var ids = Ids?.Distinct().ToList() ?? [];
        return $"{typeof(TDto).Name}-{Skip}:{Take}:{string.Join('*', ids)}:{WhereFilter?.Key}:";
    }
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
            new ValueTuple<string, object, FilterType>(queryParams.Item1, queryParams.Item2,
                GetFilterType(queryParams.Item2)));
    }

    public void Init<TValue, TViewModel>((Expression<Func<TViewModel, TValue>> predicate, TValue value) valueTuple)
    {
        var fieldsName = valueTuple.predicate.Body.ToString().Split(".").Skip(1).ToList();
        if (valueTuple.value is null) throw new ArgumentNullException("Ты чёт попутал");

        var names = string.Join(".", fieldsName);
        AddToList((names, valueTuple.value));
    }

    public void Init<TValue, TViewModel>((Expression<Func<TViewModel, TValue>> predicate, object value) valueTuple)
    {
        var fieldsName = valueTuple.predicate.Body.ToString().Replace("Convert(", string.Empty)
            .Replace(", Object)", string.Empty).Split(".").Skip(1).ToList();
        if (valueTuple.value is null) throw new ArgumentNullException("Ты чёт попутал");

        var names = string.Join(".", fieldsName);
        AddToList((names, valueTuple.value));
    }

    public ICollection<(string, object, FilterType)> List { get; } = [];

    public string Key
    {
        get { return string.Join("*", List.Select(x => $"{x.Item1}|{x.Item2}")); }
    }

    public static FilterType GetFilterType(object value)
    {
        if (value is int)
            return FilterType.Int;

        throw new NotFoundException("The filter type is not defined");
    }

    public static Type GetType(FilterType filterType)
    {
        if (filterType == FilterType.Int)
            return typeof(int);

        throw new NotFoundException("The type is not defined");
    }
}

public enum FilterType
{
    Int
}