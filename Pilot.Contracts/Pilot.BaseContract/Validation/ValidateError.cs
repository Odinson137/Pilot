namespace Pilot.Contracts.Validation;

public class ValidateError
{
    private readonly string? _error;

    public ValidateError(string value)
    {
        _error = value;
        IsSuccessfully = false;
    }

    public ValidateError()
    {
    }

    public bool IsSuccessfully { get; } = true;
    public bool IsNotSuccessfully => !IsSuccessfully;

    public string Error
    {
        get
        {
            if (string.IsNullOrEmpty(_error))
                throw new ArgumentNullException(
                    "Значение ошибки не должно здесь быть null. До этого вы должны произвести проверку на успешность валидации");

            return _error;
        }
    }
}