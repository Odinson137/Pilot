using Pilot.Contracts.DTO.ModelDto;
using Test.AuditHistory.IntegrationTests.Factories;
using Xunit.Abstractions;

namespace Test.AuditHistory.IntegrationTests;

public class AuditHistoryTests(AuditHistoryTestAuditHistoryFactory factory, ITestOutputHelper testOutputHelper)
    : BaseModelIntegrationTest<Pilot.AuditHistory.Models.AuditHistory, AuditHistoryDto>(factory, testOutputHelper)
{

}