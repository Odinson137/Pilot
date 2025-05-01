using Pilot.Contracts.Base;

namespace Pilot.Contracts.DTO.TransferServiceDto;

// служит для того, чтоб создать задачу после того, как статус заявки сотрудника был переведён в статус Approved
public class AppliedStatusDto : BaseDto
{
    public int CompanyId { get; set; }
    
    public int UserId { get; set; }
    
    public int PostId { get; set; }
}