using Xunit.Abstractions;

namespace Test.Receiver;

public class UnitTest(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public void Test1()
    {
        testOutputHelper.WriteLine("Test work");
    }
}