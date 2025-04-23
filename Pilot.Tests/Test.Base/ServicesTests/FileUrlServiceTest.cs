// using System.Reflection;
// using JetBrains.Annotations;
// using Microsoft.Extensions.Logging;
// using Moq;
// using Pilot.Api.Services;
// using Pilot.Contracts.Attributes;
// using Pilot.Contracts.Base;
// using Pilot.Contracts.DTO.ModelDto;
// using Pilot.Contracts.Interfaces;
//
// namespace Test.Base.ServicesTests;
//
// [TestSubject(typeof(FileService))]
// public class FileUrlServiceTest
// {
//     public static IEnumerable<object[]> ModelData()
//     {
//         var dtoTypes = Assembly.GetAssembly(typeof(BaseDto))
//             ?.GetTypes()
//             .Where(t => t.IsClass && t.GetInterfaces().Contains(typeof(IHasFile))) 
//             .ToArray();
//
//         if (dtoTypes == null) yield break;
//
//         foreach (var dtoType in dtoTypes)
//         {
//             yield return [dtoType];
//         }
//     }
//
//     [Theory]
//     [MemberData(nameof(ModelData))]
//     public async Task GetUrlAsync_Should_Fill_FileFields_Correctly(Type dtoType)
//     {
//         // Arrange
//         var loggerMock = new Mock<ILogger<FileService>>();
//         var modelServiceMock = new Mock<IModelService>();
//         var baseMassTransitServiceMock = new Mock<IBaseMassTransitService>();
//
//         var filesList = new List<FileDto>();
//
//         var dtoInstance = Activator.CreateInstance(dtoType);
//
//         var fileProperties = dtoType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
//             .Where(p => p.IsDefined(typeof(FileAttribute), false))
//             .ToList();
//
//         var id = 1;
//         foreach (var property in fileProperties)
//         {
//             var attribute = property.GetCustomAttribute<FileAttribute>();
//
//             if (attribute == null) continue;
//             if (property.PropertyType == typeof(string))
//             {
//                 var fileDto = new FileDto { Id = id, Name = $"{Guid.NewGuid()}", Url = $"https://example.com/file{id}.jpg" };
//                 filesList.Add(fileDto);
//                 property.SetValue(dtoInstance, fileDto.Name);
//             }
//             else if (property.PropertyType == typeof(List<string>) || property.PropertyType == typeof(ICollection<string>))
//             {
//                 var fileDto = new FileDto { Id = id, Name = $"{Guid.NewGuid()}", Url = $"https://example.com/file{id}.jpg" };
//                 filesList.Add(fileDto);
//                 property.SetValue(dtoInstance, new List<string> { fileDto.Name });
//             }
//             id++;
//         }
//
//         modelServiceMock
//             .Setup(x => x.GetValuesAsync<FileDto>(It.IsAny<string>(), It.IsAny<BaseFilter>(), default))
//             .ReturnsAsync(filesList);
//
//         var service = new FileService(loggerMock.Object, modelServiceMock.Object, baseMassTransitServiceMock.Object);
//         
//         // Act
//         await service.GetUrlAsync(dtoInstance!);
//
//         // Assert
//         foreach (var property in fileProperties)
//         {
//             if (property.PropertyType == typeof(string))
//             {
//                 var urlValue = property.GetValue(dtoInstance) as string;
//                 Assert.NotNull(urlValue);
//                 Assert.NotEmpty(urlValue);
//             }
//             else if (property.PropertyType == typeof(List<string>) || property.PropertyType == typeof(ICollection<string>))
//             {
//                 var urlList = property.GetValue(dtoInstance) as ICollection<string>;
//                 Assert.NotNull(urlList);
//                 Assert.NotEmpty(urlList);
//             }
//         }
//     }
//     
//     [Theory]
//     [MemberData(nameof(ModelData))]
//     public async Task GetUrlAsync_Should_Fill_FileFieldsForCollection_Correctly(Type dtoType)
//     {
//         // Arrange
//         var loggerMock = new Mock<ILogger<FileService>>();
//         var modelServiceMock = new Mock<IModelService>();
//         var baseMassTransitServiceMock = new Mock<IBaseMassTransitService>();
//
//         var filesList = new List<FileDto>();
//
//         var dtoCollection = new List<object> { Activator.CreateInstance(dtoType)! };
//
//         var fileProperties = dtoType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
//             .Where(p => p.IsDefined(typeof(FileAttribute), false))
//             .ToList();
//
//         var id = 1;
//         foreach (var dtoInstance in dtoCollection)
//         {
//             foreach (var property in fileProperties)
//             {
//                 var attribute = property.GetCustomAttribute<FileAttribute>();
//
//                 if (attribute == null) continue;
//                 if (property.PropertyType == typeof(string))
//                 {
//                     var fileDto = new FileDto { Id = id, Name = Guid.NewGuid().ToString(), Url = $"https://example.com/file{id}.jpg" };
//                     filesList.Add(fileDto);
//                     property.SetValue(dtoInstance, fileDto.Name);
//                 }
//                 else if (property.PropertyType == typeof(List<string>) || property.PropertyType == typeof(ICollection<string>))
//                 {
//                     var fileDto = new FileDto { Id = id, Name = Guid.NewGuid().ToString(), Url = $"https://example.com/file{id}.jpg" };
//                     filesList.Add(fileDto);
//                     property.SetValue(dtoInstance, new List<string> { fileDto.Name });
//                 }
//                 id++;
//             }
//         }
//
//         modelServiceMock
//             .Setup(x => x.GetValuesAsync<FileDto>(It.IsAny<string>(), It.IsAny<BaseFilter>(), default))
//             .ReturnsAsync(filesList);
//         
//         var service = new FileService(loggerMock.Object, modelServiceMock.Object, baseMassTransitServiceMock.Object);
//
//         // Act
//         await service.GetUrlAsync(dtoCollection);
//
//         // Assert
//         foreach (var dtoInstance in dtoCollection)
//         {
//             foreach (var property in fileProperties)
//             {
//                 if (property.PropertyType == typeof(int?))
//                 {
//                     var urlValue = property?.GetValue(dtoInstance) as string;
//                     
//                     Assert.NotNull(urlValue);
//                     Assert.NotEmpty(urlValue);
//                 }
//                 else if (property.PropertyType == typeof(List<int>) || property.PropertyType == typeof(ICollection<int>))
//                 {
//                     var urlList = property.GetValue(dtoInstance) as ICollection<string>;
//
//                     Assert.NotNull(urlList);
//                     Assert.NotEmpty(urlList);
//                 }
//             }
//         }
//     }
// }
