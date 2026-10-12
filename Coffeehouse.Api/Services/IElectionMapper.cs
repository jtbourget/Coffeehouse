using Coffeehouse.Api.DTOs;
using Coffeehouse.Api.Models;

namespace Coffeehouse.Api.Services;

/// <summary>
/// Defines mapping operations between election entities and data transfer objects.
/// </summary>
public interface IElectionMapper
{
    ElectionResponseDto ToDto(Election entity);
}
