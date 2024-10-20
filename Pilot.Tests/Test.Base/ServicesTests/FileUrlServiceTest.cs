using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Moq;
using Pilot.Api.Services;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;

namespace Test.Base.ServicesTests;

[TestSubject(typeof(FileUrlService))]
public class FileUrlServiceTest
{
    public static IEnumerable<object[]> ModelData()
    {
        var dtoTypes = Assembly.GetAssembly(typeof(BaseDto))
            ?.GetTypes()
            .Where(t => t.IsClass && t.GetInterfaces().Contains(typeof(IHasFile))) 
            .ToArray();

        if (dtoTypes == null) yield break;

        foreach (var dtoType in dtoTypes)
        {
            yield return [dtoType];
        }
    }

    [Theory]
    [MemberData(nameof(ModelData))]
    public async Task GetUrlAsync_Should_Fill_FileFields_Correctly(Type dtoType)
    {
        // Arrange
        var loggerMock = new Mock<ILogger<FileUrlService>>();
        var modelServiceMock = new Mock<IModelService>();

        var filesList = new List<FileDto>();

        var dtoInstance = Activator.CreateInstance(dtoType);

        var fileProperties = dtoType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.IsDefined(typeof(HasFileAttribute), false))
            .ToList();

        var id = 1;
        foreach (var property in fileProperties)
        {
            var attribute = property.GetCustomAttribute<HasFileAttribute>();

            if (attribute == null) continue;
            if (property.PropertyType == typeof(int?))
            {
                var fileDto = new FileDto { Id = id, Url = $"https://example.com/file{id}.jpg" };
                filesList.Add(fileDto);
                property.SetValue(dtoInstance, fileDto.Id);
            }
            else if (property.PropertyType == typeof(List<int>) || property.PropertyType == typeof(ICollection<int>))
            {
                var fileDto = new FileDto { Id = id, Url = $"https://example.com/file{id}.jpg" };
                filesList.Add(fileDto);
                property.SetValue(dtoInstance, new List<int> { fileDto.Id });
            }
            id++;
        }

        modelServiceMock
            .Setup(x => x.GetValuesAsync<FileDto>(It.IsAny<string>(), It.IsAny<BaseFilter>(), default))
            .ReturnsAsync(filesList);

        var service = new FileUrlService(loggerMock.Object, modelServiceMock.Object);
        
        // Act
        await service.GetUrlAsync(dtoInstance!);

        // Assert
        foreach (var property in fileProperties)
        {
            var attribute = property.GetCustomAttribute<HasFileAttribute>();
            var urlFieldName = attribute!.FieldName;

            var urlField = dtoType.GetProperty(urlFieldName);

            if (property.PropertyType == typeof(int?))
            {
                var urlValue = urlField?.GetValue(dtoInstance) as string;
                var fileDto = filesList.First(f => f.Id == (int?)property.GetValue(dtoInstance));
                Assert.Equal(fileDto.Url, urlValue);
            }
            else if (property.PropertyType == typeof(List<int>) || property.PropertyType == typeof(ICollection<int>))
            {
                var urlList = urlField?.GetValue(dtoInstance) as ICollection<string>;
                var ids = property.GetValue(dtoInstance) as ICollection<int>;

                foreach (var idValue in ids!)
                {
                    var fileDto = filesList.First(f => f.Id == idValue);
                    Assert.Contains(fileDto.Url, urlList!);
                }
            }
        }
    }
    
    [Theory]
    [MemberData(nameof(ModelData))]
    public async Task GetUrlAsync_Should_Fill_FileFieldsForCollection_Correctly(Type dtoType)
    {
        // Arrange
        var loggerMock = new Mock<ILogger<FileUrlService>>();
        var modelServiceMock = new Mock<IModelService>();

        var filesList = new List<FileDto>();

        var dtoCollection = new List<object> { Activator.CreateInstance(dtoType)! };

        var fileProperties = dtoType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.IsDefined(typeof(HasFileAttribute), false))
            .ToList();

        var id = 1;
        foreach (var dtoInstance in dtoCollection)
        {
            foreach (var property in fileProperties)
            {
                var attribute = property.GetCustomAttribute<HasFileAttribute>();

                if (attribute == null) continue;
                if (property.PropertyType == typeof(int?))
                {
                    var fileDto = new FileDto { Id = id, Url = $"https://example.com/file{id}.jpg" };
                    filesList.Add(fileDto);
                    property.SetValue(dtoInstance, fileDto.Id);
                }
                else if (property.PropertyType == typeof(List<int>) || property.PropertyType == typeof(ICollection<int>))
                {
                    var fileDto = new FileDto { Id = id, Url = $"https://example.com/file{id}.jpg" };
                    filesList.Add(fileDto);
                    property.SetValue(dtoInstance, new List<int> { fileDto.Id });
                }
                id++;
            }
        }

        modelServiceMock
            .Setup(x => x.GetValuesAsync<FileDto>(It.IsAny<string>(), It.IsAny<BaseFilter>(), default))
            .ReturnsAsync(filesList);

        var service = new FileUrlService(loggerMock.Object, modelServiceMock.Object);

        // Act
        await service.GetUrlAsync(dtoCollection);

        // Assert
        foreach (var dtoInstance in dtoCollection)
        {
            foreach (var property in fileProperties)
            {
                var attribute = property.GetCustomAttribute<HasFileAttribute>();
                var urlFieldName = attribute!.FieldName;

                var urlField = dtoType.GetProperty(urlFieldName);

                if (property.PropertyType == typeof(int?))
                {
                    var urlValue = urlField?.GetValue(dtoInstance) as string;
                    var fileDto = filesList.First(f => f.Id == (int?)property.GetValue(dtoInstance));
                    Assert.Equal(fileDto.Url, urlValue);
                }
                else if (property.PropertyType == typeof(List<int>) || property.PropertyType == typeof(ICollection<int>))
                {
                    var urlList = urlField?.GetValue(dtoInstance) as ICollection<string>;
                    var ids = property.GetValue(dtoInstance) as ICollection<int>;

                    foreach (var idValue in ids!)
                    {
                        var fileDto = filesList.First(f => f.Id == idValue);
                        Assert.Contains(fileDto.Url, urlList!);
                    }
                }
            }
        }
    }
}
