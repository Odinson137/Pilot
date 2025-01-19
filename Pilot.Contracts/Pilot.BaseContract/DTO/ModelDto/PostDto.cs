using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.DTO.ModelDto;

[FromService(ServiceName.CapabilityServer)]
public class PostDto : BaseDto
{
    [Required] [MaxLength(100)] public string Title { get; set; } = null!;

    [MaxLength(500)] public string? Description { get; set; }

    [Required] [Range(1, int.MaxValue)] public int CompanyId { get; set; }

    public ICollection<BaseDto> Skills { get; set; } = [];

    public ICollection<BaseDto> CompanyPosts { get; set; } = [];
}