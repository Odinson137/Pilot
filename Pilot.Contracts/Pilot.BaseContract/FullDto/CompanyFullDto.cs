using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Validation.ValidationAttributes;

namespace Pilot.Contracts.FullDto;

[FromService(ServiceName.WorkerServer)]
public class CompanyFullDto : BaseDto, IHasFile
{
    [Required]
    [MaxLength(50)]
    [CheckNameExist]
    public string Title { get; set; } = null!;

    [MaxLength(500)] public string? Description { get; init; }

    public ICollection<ProjectFullDto> Projects { get; init; } = [];

    public ICollection<CompanyUserFullDto> CompanyUsers { get; set; } = [];
    
    [File]
    public string? Logo { get; set; }

    [File]
    public ICollection<string> InsideImages { get; set; } = [];
}