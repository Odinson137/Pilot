using System.ComponentModel.DataAnnotations;

namespace Pilot.Contracts.Base;

public abstract class BaseModel : BaseId
{
    [Key]
    public new int Id { get; init; }
}