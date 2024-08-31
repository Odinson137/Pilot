namespace Pilot.Contracts.Services;

public static class StringExtensions
{
    public static string TakeOnly(this string value, int length)
    {
        return value.Length > length ? value[..length] : value;
    }
}