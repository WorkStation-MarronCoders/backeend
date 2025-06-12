using workstation_backend.Shared.Domain.Repositories;

namespace workstation_backend.OfficesContext.Domain;

using OfficesContext.Domain.Models.Entities;

public interface IRatingRepository : IBaseRepository<Rating>
{
    Task<IEnumerable<Rating>> GetByOfficeIdAsync(Guid officeId);

}