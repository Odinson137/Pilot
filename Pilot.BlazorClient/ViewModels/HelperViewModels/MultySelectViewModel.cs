namespace Pilot.BlazorClient.ViewModels.HelperViewModels;

public class MultySelectViewModel<T>
{
    public required T Value { get; set; }
    public bool IsSelected { get; set; }

    public static ICollection<MultySelectViewModel<T>> GetList(ICollection<T> startList)
    {
        return startList.Select(c => new MultySelectViewModel<T> { Value = c }).ToList();
    }
}