﻿namespace Pilot.Contracts.DTO;

public class AuthorizationUserDto
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
}