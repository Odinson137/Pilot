namespace Pilot.Contracts.Base;

public class BaseId
{
    public virtual int Id { get; init; }
    
    public virtual DateTime CreateAt { get; set; }
    
    public virtual DateTime ChangeAt { get; set; }
    
    public virtual DateTime DeleteAt { get; set; }
}