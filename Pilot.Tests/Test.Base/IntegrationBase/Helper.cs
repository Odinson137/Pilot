namespace Test.Base.IntegrationBase;

public static class Helper
{
    // Маленькое ухищрение, которое позволяет ожидать пока консюмер обработает сообщение из очереди,
    // А ещё нормально использовать debug, имея в запасе i кликов в нём
    public static async Task Wait()
    {
        for (var i = 0; i < 40; i++) await Task.Delay(100);
    }
}