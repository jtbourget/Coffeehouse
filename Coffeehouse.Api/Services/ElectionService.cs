using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coffeehouse.Api.DTOs;
using Coffeehouse.Api.Repositories;
using Microsoft.Extensions.Logging;

namespace Coffeehouse.Api.Services;

/// <summary>
/// Scoped service for election operations.
/// Depends on IElectionRepository.
/// </summary>
public class ElectionService : IElectionService
{
    private readonly IElectionRepository _repository;
    private readonly IElectionMapper _mapper;
    private readonly ILogger<ElectionService> _logger;

    public ElectionService(IElectionRepository repository, IElectionMapper mapper, ILogger<ElectionService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<ElectionResponseDto>> GetAllElectionsAsync()
    {
        _logger.LogDebug("Retrieving all elections.");
        var elections = await _repository.GetAllAsync();
        return elections.Select(_mapper.ToDto);
    }

    public async Task<ElectionResponseDto?> GetElectionByIdAsync(int id)
    {
        _logger.LogDebug("Retrieving election with ID {Id}.", id);
        var election = await _repository.GetByIdAsync(id);
        if (election == null)
        {
            _logger.LogInformation("Election with ID {Id} was not found.", id);
            return null;
        }

        return _mapper.ToDto(election);
    }
}
