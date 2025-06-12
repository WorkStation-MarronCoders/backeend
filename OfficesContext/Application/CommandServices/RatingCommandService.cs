using workstation_backend.OfficesContext.Domain;
using workstation_backend.OfficesContext.Domain.Models.Commands;
using workstation_backend.Shared.Domain.Repositories;

namespace workstation_backend.OfficesContext.Application.CommandServices;

public class RatingCommandService
{
    private readonly IRatingRepository _ratingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RatingCommandService(IRatingRepository ratingRepository, IUnitOfWork unitOfWork)
    {
        _ratingRepository = ratingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Rating> CreateRatingAsync(CreateRatingCommand command)
    {
        var rating = new Rating
        {
            Id = Guid.NewGuid(),
            Score = command.Score,
            Comment = command.Comment,
            OfficeId = command.OfficeId
        };

        await _ratingRepository.AddAsync(rating);
        await _unitOfWork.CompleteAsync();

        return rating;
    }
}
