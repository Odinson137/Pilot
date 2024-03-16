using Microsoft.AspNetCore.Mvc;

namespace Pilot.Api.Data.ControllerSettings;

public class PilotController : ControllerBase
{
    protected string UserId => User.Identities.First().Claims.First().Value;
}