using System.Collections.Generic;
using System.Threading.Tasks;
using Coffeehouse.Api.DTOs;

namespace Coffeehouse.Api.Services;

/// <summary>
/// Service interface for election operations.
/// </summary>
public interface IElectionService
{
    Task<IEnumerable<ElectionResponseDto>> GetAllElectionsAsync();
    Task<ElectionResponseDto?> GetElectionByIdAsync(int id);
}
