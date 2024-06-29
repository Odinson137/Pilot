using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;

namespace Pilot.Contracts.SecondGenerationBase;

public class NamedModel : BaseModel
{
    [Required] public required string Name { get; set; } = null!;
}