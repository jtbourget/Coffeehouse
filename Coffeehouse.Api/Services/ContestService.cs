using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coffeehouse.Api.DTOs;
using Coffeehouse.Api.Repositories;
using Microsoft.Extensions.Logging;

namespace Coffeehouse.Api.Services;

/// <summary>
/// Scoped service for contest operations.
/// Depends on IContestRepository.
/// </summary>
public class ContestService : IContestService
{
    private readonly IContestRepository _repository;
    private readonly IContestMapper _mapper;
    private readonly ILogger<ContestService> _logger;

    public ContestService(IContestRepository repository, IContestMapper mapper, ILogger<ContestService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<ContestResponseDto>?> GetContestsForElectionAsync(int electionId)
    {
        _logger.LogDebug("Retrieving contests for election ID {ElectionId}.", electionId);
        var electionExists = await _repository.ElectionExistsAsync(electionId);
        if (!electionExists)
        {
            _logger.LogInformation("Election ID {ElectionId} does not exist.", electionId);
            return null;
        }

        var contests = await _repository.GetContestsByElectionIdAsync(electionId);
        return contests.Select(_mapper.ToDto);
    }
}
