using workstation_backend.OfficesContext.Domain;
using workstation_backend.OfficesContext.Domain.Models.Commands;
using workstation_backend.Shared.Domain.Repositories;

namespace workstation_backend.OfficesContext.Application.CommandServices;

/// <summary>
/// Servicio encargado de manejar los comandos relacionados con calificaciones (<see cref="Rating"/>).
/// </summary>
public class RatingCommandService
{
    private readonly IRatingRepository _ratingRepository;

    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Inicializa una nueva instancia del servicio de comandos de calificaciones.
    /// </summary>
    /// <param name="ratingRepository">Repositorio de calificaciones.</param>
    /// <param name="unitOfWork">Unidad de trabajo para confirmar cambios.</param>
    public RatingCommandService(IRatingRepository ratingRepository, IUnitOfWork unitOfWork)
    {
        _ratingRepository = ratingRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Crea una nueva calificación para una oficina.
    /// </summary>
    /// <param name="command">Comando con los datos de la calificación.</param>
    /// <returns>La calificación creada.</returns>
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
