using Pilot.Contracts.Base;

namespace Pilot.Contracts.DTO.ModelDto;

public class FileDto : BaseDto
{
    public required string Url { get; set; }
    public required string Type { get; set; }
}