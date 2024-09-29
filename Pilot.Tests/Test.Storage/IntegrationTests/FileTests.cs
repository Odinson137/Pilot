using Pilot.Contracts.DTO.ModelDto;
using Test.Storage.IntegrationTests.Factories;
using Xunit.Abstractions;
using File = Pilot.Storage.Models.File;

namespace Test.Storage.IntegrationTests;

public class FileTests(StorageTestStorageFactory factory, ITestOutputHelper testOutputHelper)
    : BaseModelIntegrationTest<File, FileDto>(factory, testOutputHelper);