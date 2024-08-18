using System.ComponentModel.DataAnnotations;

namespace Pilot.Contracts.Base;

public class BaseModel : BaseId
{
    [Key] public override int Id { get; set; }

    public override DateTime? CreateAt { get; set; } = DateTime.Now;
}