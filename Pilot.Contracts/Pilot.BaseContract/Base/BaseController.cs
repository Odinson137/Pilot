using Microsoft.AspNetCore.Mvc;

namespace Pilot.Contracts.Base;

public class BaseController : ControllerBase
{
    protected int UserId => int.Parse(User.Identities.First().Claims.First().Value);
}