using Xunit.Abstractions;

namespace Test.Base;

public class UnitTest1(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public void Test1()
    {
        testOutputHelper.WriteLine("Test work");
    }
}