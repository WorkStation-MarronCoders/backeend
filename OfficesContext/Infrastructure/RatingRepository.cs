namespace workstation_backend.OfficesContext.Infrastructure;

using Microsoft.EntityFrameworkCore;
using workstation_backend.OfficesContext.Domain;
using workstation_backend.OfficesContext.Domain.Models.Entities;
using workstation_backend.Shared.Infrastructure.Persistence.Configuration;
using workstation_backend.Shared.Infrastructure.Persistence.Repositories;

using OfficesContext.Domain;
using Shared.Infrastructure.Persistence.Configuration;


public class RatingRepository : IRatingRepository
{
    private readonly WorkstationContext _context;

    public RatingRepository(WorkstationContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Rating>> GetByOfficeIdAsync(Guid officeId)
    {
        return await _context.Ratings
            .Where(r => r.OfficeId == officeId)  
            .ToListAsync();
    }

    public async Task AddAsync(Rating entity) => await _context.Ratings.AddAsync(entity);
    public Task<Rating?> FindByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(Rating entity)
    {
        throw new NotImplementedException();
    }

    public void Remove(Rating entity) => _context.Ratings.Remove(entity);
    public Task<IEnumerable<Rating>> ListAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Rating?> GetByIdAsync(Guid id) => _context.Ratings.FirstOrDefaultAsync(r=> r.Id == id);

    public Task<IEnumerable<Rating>> GetAllAsync() =>
        Task.FromResult<IEnumerable<Rating>>(_context.Ratings.ToList());
}
