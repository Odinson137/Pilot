using System.Reflection;
using JetBrains.Annotations;
using Pilot.Contracts.Base;
using Pilot.Contracts.Services;

namespace Test.Base.ServicesTests;

[TestSubject(typeof(HttpNameService))]
public class HttpNameServiceTest
{
    public static IEnumerable<object[]> ModelData
    {
        get
        {
            var baseModelDtoType = typeof(BaseDto);
            var assembly = Assembly.GetAssembly(baseModelDtoType);

            var modelTypes = assembly?.GetTypes()
                .Where(t => t is { IsClass: true, IsAbstract: false } && t.IsSubclassOf(baseModelDtoType))
                .Select(c => new object[] { c })
                .ToList();

            return modelTypes!;
        }
    }
    
    [Theory]
    [MemberData(nameof(ModelData))]
    public void GetHttpClientName_Test(Type type)
    {
        // Act
        var httpClientName = HttpNameService.GetHttpClientName(type);
        
        // Assert
        Assert.NotNull(httpClientName);
    }
}