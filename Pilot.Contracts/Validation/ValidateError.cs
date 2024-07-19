namespace Pilot.Contracts.Validation;

public class ValidateError
{
    public bool IsSuccessfully { get; }
    public bool IsNotSuccessfully => !IsSuccessfully;

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

    public ValidateError(string value)
    {
        _error = value;
        IsSuccessfully = false;
    }

    public ValidateError() { }
}