using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coffeehouse.Api.DTOs;
using Coffeehouse.Api.Repositories;

namespace Coffeehouse.Api.Services;

/// <summary>
/// Scoped service for candidate operations.
/// Depends on ICandidateRepository.
/// </summary>
public class CandidateService : ICandidateService
{
    private readonly ICandidateRepository _repository;
    private readonly ICandidateMapper _mapper;

    public CandidateService(ICandidateRepository repository, ICandidateMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CandidateResponseDto>?> GetCandidatesForContestAsync(int contestId)
    {
        var contestExists = await _repository.ContestExistsAsync(contestId);
        if (!contestExists)
        {
            return null;
        }

        var candidates = await _repository.GetCandidatesByContestIdAsync(contestId);
        return candidates.Select(_mapper.ToDto);
    }

    public async Task<CandidateResponseDto?> GetCandidateAsync(int id)
    {
        var candidate = await _repository.GetByIdAsync(id);
        return candidate == null ? null : _mapper.ToDto(candidate);
    }
}
