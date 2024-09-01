namespace Pilot.Contracts.Services.LogService;

public static class ByteExtensions
{
    public static double GetSize(this byte[]? byteFile) => byteFile?.Length / 1024.0 ?? throw new System.Exception("Нет файла, чтоб узнать его длину");
}