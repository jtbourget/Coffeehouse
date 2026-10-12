using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coffeehouse.Api.DTOs;
using Coffeehouse.Api.Repositories;

namespace Coffeehouse.Api.Services;

/// <summary>
/// Scoped service for contest operations.
/// Depends on IContestRepository.
/// </summary>
public class ContestService : IContestService
{
    private readonly IContestRepository _repository;
    private readonly IContestMapper _mapper;

    public ContestService(IContestRepository repository, IContestMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ContestResponseDto>?> GetContestsForElectionAsync(int electionId)
    {
        var electionExists = await _repository.ElectionExistsAsync(electionId);
        if (!electionExists)
        {
            return null;
        }

        var contests = await _repository.GetContestsByElectionIdAsync(electionId);
        return contests.Select(_mapper.ToDto);
    }
}
