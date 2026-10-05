import os
import re

api_dir = r"C:\Users\joshu\source\repos\Coffeehouse\Coffeehouse.Api"
controllers_dir = os.path.join(api_dir, "Controllers")
repos_dir = os.path.join(api_dir, "Repositories")
services_dir = os.path.join(api_dir, "Services")
dtos_dir = os.path.join(api_dir, "Models", "DTOs")

entities = ["Election", "Contest", "Candidate", "CandidateProfile", "FavoriteCandidate"]
# We already have CandidateProfileMapper, FavoritesMapper from the subagents? Let's check if they exist.

def ensure_dir(d):
    if not os.path.exists(d):
        os.makedirs(d)

ensure_dir(repos_dir)
ensure_dir(services_dir)
ensure_dir(dtos_dir)

# A simplified generator script
for entity in entities:
    # 1. DTO
    dto_content = f"""namespace Coffeehouse.Api.Models.DTOs;

public class {entity}ResponseDto
{{
    public int Id {{ get; set; }}
    public string Name {{ get; set; }} = string.Empty;
}}
"""
    dto_path = os.path.join(dtos_dir, f"{entity}ResponseDto.cs")
    if not os.path.exists(dto_path):
        with open(dto_path, "w") as f: f.write(dto_content)

    # 2. Repo Interface
    irepo_content = f"""using Coffeehouse.Api.Models;

namespace Coffeehouse.Api.Repositories;

public interface I{entity}Repository
{{
    Task<IEnumerable<{entity}>> GetAllAsync();
    Task<{entity}?> GetByIdAsync(int id);
}}
"""
    irepo_path = os.path.join(repos_dir, f"I{entity}Repository.cs")
    if not os.path.exists(irepo_path):
        with open(irepo_path, "w") as f: f.write(irepo_content)

    # 3. Sql Repo
    sqlrepo_content = f"""using Coffeehouse.Api.Data;
using Coffeehouse.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Coffeehouse.Api.Repositories;

public class Sql{entity}Repository : I{entity}Repository
{{
    private readonly AppDbContext _context;

    public Sql{entity}Repository(AppDbContext context)
    {{
        _context = context;
    }}

    public async Task<IEnumerable<{entity}>> GetAllAsync()
    {{
        return await _context.Set<{entity}>().ToListAsync();
    }}

    public async Task<{entity}?> GetByIdAsync(int id)
    {{
        return await _context.Set<{entity}>().FindAsync(id);
    }}
}}
"""
    sqlrepo_path = os.path.join(repos_dir, f"Sql{entity}Repository.cs")
    if not os.path.exists(sqlrepo_path):
        with open(sqlrepo_path, "w") as f: f.write(sqlrepo_content)

    # 4. Mapper Interface
    imapper_content = f"""using Coffeehouse.Api.Models;
using Coffeehouse.Api.Models.DTOs;

namespace Coffeehouse.Api.Services;

public interface I{entity}Mapper
{{
    {entity}ResponseDto ToDto({entity} entity);
}}
"""
    imapper_path = os.path.join(services_dir, f"I{entity}Mapper.cs")
    if not os.path.exists(imapper_path):
        with open(imapper_path, "w") as f: f.write(imapper_content)

    # 5. Mapper
    mapper_content = f"""using Coffeehouse.Api.Models;
using Coffeehouse.Api.Models.DTOs;

namespace Coffeehouse.Api.Services;

public class {entity}Mapper : I{entity}Mapper
{{
    public {entity}ResponseDto ToDto({entity} entity)
    {{
        return new {entity}ResponseDto
        {{
            Id = entity.Id,
        }};
    }}
}}
"""
    mapper_path = os.path.join(services_dir, f"{entity}Mapper.cs")
    if not os.path.exists(mapper_path):
        with open(mapper_path, "w") as f: f.write(mapper_content)

print("Scaffolding complete.")
