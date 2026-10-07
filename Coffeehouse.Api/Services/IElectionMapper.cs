using Coffeehouse.Api.Models;
using Coffeehouse.Api.Models.DTOs;

namespace Coffeehouse.Api.Services;

/// <summary>
/// Defines mapping operations between election entities and data transfer objects.
/// </summary>
public interface IElectionMapper
{
    ElectionResponseDto ToDto(Election entity);
}
