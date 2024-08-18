using Microsoft.AspNetCore.Builder;

namespace Pilot.Contracts.Data;

public interface ISeed
{
    public Task Seeding(IApplicationBuilder app);
}