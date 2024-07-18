namespace Pilot.Contracts.Validation;

public class AttributeError
{
    public bool IsSuccessfully { get; }
    public bool IsNotSuccessfully { get; }

    private readonly string? _error;
    public string Error
    {
        get
        {
            if (string.IsNullOrEmpty(_error))
            {
                throw new ArgumentNullException($"Значение ошибки не должно здесь быть null. До этого вы должны произвести проверку на успешность валидации");
            }

            return _error;
        }
    }

    public AttributeError(string value)
    {
        _error = value;
        IsSuccessfully = false;
        IsNotSuccessfully = true;
    }

    public AttributeError() { }
}