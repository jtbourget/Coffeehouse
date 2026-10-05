using System;
using System.Collections.Generic;

namespace Coffeehouse.Api.Models.DTOs;

public class ElectionResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime ElectionDate { get; set; }
    public string VotingLocationName { get; set; } = string.Empty;
    public string VotingLocationAddress { get; set; } = string.Empty;
    public List<ContestResponseDto> Contests { get; set; } = new();
}
