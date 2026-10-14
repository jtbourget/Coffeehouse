using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coffeehouse.Api.DTOs;
using Coffeehouse.Api.Repositories;
using Microsoft.Extensions.Logging;

namespace Coffeehouse.Api.Services;

/// <summary>
/// Scoped service for candidate operations.
/// Depends on ICandidateRepository.
/// </summary>
public class CandidateService : ICandidateService
{
    private readonly ICandidateRepository _repository;
    private readonly ICandidateMapper _mapper;
    private readonly ILogger<CandidateService> _logger;

    public CandidateService(ICandidateRepository repository, ICandidateMapper mapper, ILogger<CandidateService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<CandidateResponseDto>?> GetCandidatesForContestAsync(int contestId)
    {
        _logger.LogDebug("Retrieving candidates for contest ID {ContestId}.", contestId);
        var contestExists = await _repository.ContestExistsAsync(contestId);
        if (!contestExists)
        {
            _logger.LogInformation("Contest ID {ContestId} does not exist.", contestId);
            return null;
        }

        var candidates = await _repository.GetCandidatesByContestIdAsync(contestId);
        return candidates.Select(_mapper.ToDto);
    }

    public async Task<CandidateResponseDto?> GetCandidateAsync(int id)
    {
        _logger.LogDebug("Retrieving candidate with ID {Id}.", id);
        var candidate = await _repository.GetByIdAsync(id);
        if (candidate == null)
        {
            _logger.LogInformation("Candidate with ID {Id} was not found.", id);
            return null;
        }

        return _mapper.ToDto(candidate);
    }
}
