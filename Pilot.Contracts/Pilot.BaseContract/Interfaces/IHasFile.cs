namespace Pilot.Contracts.Interfaces;

// чтоб не проходиться по всем свойствам каждый раз
public interface IHasFile // пустой интерфейс, который будет просто говорить коду, если ли в обьекте файлы
{
    // теперь интерфейс не пустой)
    // Key - Property Name, Value - File
    public Dictionary<string, ICollection<byte[]>>? Files { get; set; }
}