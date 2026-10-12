using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coffeehouse.Api.DTOs;
using Coffeehouse.Api.Repositories;

namespace Coffeehouse.Api.Services;

/// <summary>
/// Scoped service for election operations.
/// Depends on IElectionRepository.
/// </summary>
public class ElectionService : IElectionService
{
    private readonly IElectionRepository _repository;
    private readonly IElectionMapper _mapper;

    public ElectionService(IElectionRepository repository, IElectionMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ElectionResponseDto>> GetAllElectionsAsync()
    {
        var elections = await _repository.GetAllAsync();
        return elections.Select(_mapper.ToDto);
    }

    public async Task<ElectionResponseDto?> GetElectionByIdAsync(int id)
    {
        var election = await _repository.GetByIdAsync(id);
        return election == null ? null : _mapper.ToDto(election);
    }
}
