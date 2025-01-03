﻿using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Validation.ValidationAttributes;

namespace Pilot.Contracts.DTO.ModelDto;

[FromService(ServiceName.WorkerServer)]
public class CompanyDto : BaseDto, IHasFile
{
    [Required]
    [MaxLength(50)]
    [CheckNameExist]
    public string Title { get; set; } = null!;

    [MaxLength(500)] public string? Description { get; init; }

    public ICollection<BaseDto> Projects { get; init; } = [];

    public ICollection<BaseDto> CompanyRoles { get; set; } = [];
    
    public ICollection<BaseDto> CompanyUsers { get; set; } = [];
    
    [HasFile(nameof(LogoUrl))]
    public int? LogoId { get; set; }

    public string? LogoUrl { get; set; }

    [HasFile(nameof(InsideImagesUrl))]
    public ICollection<int> InsideImagesId { get; set; } = [];

    public ICollection<string> InsideImagesUrl { get; set; } = [];
}