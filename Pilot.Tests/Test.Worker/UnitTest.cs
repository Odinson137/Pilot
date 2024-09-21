using Xunit.Abstractions;

namespace Test.Worker;

public class UnitTest(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public void Test1()
    {
        testOutputHelper.WriteLine("Test work");
    }
}