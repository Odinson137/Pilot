using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.SqrsControllerLibrary.Interfaces;

namespace Pilot.AuditHistory.Models;

// как будто бы BaseModel здесь не нужно, но ради единообразия я оставил
public class AuditHistory : BaseModel, IAddUser
{
    [Required] public int UserId { get; set; }

    [Required] public int EntityId { get; set; }
    
    [Required] public ModelType EntityType { get; set; }

    [Required] [MaxLength(1000)] public string NewValue { get; set; } = null!;
    
    [Required] public ActionState ActionState { get; set; }

    public void AddUser(int userId)
    {
        UserId = userId;
    }
}