using Pilot.Contracts.DTO.ModelDto;
using Test.Storage.IntegrationTests.Factories;
using File = Pilot.Storage.Models.File;

namespace Test.Storage.IntegrationTests;

public class FileTests(FileTestStorageFactory factory)
    : BaseModelReceiverIntegrationTest<File, FileDto>(factory);