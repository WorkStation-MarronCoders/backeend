using Microsoft.AspNetCore.Mvc;
using workstation_backend.OfficesContext.Application.CommandServices;
using workstation_backend.OfficesContext.Domain.Models.Commands;
using workstation_backend.OfficesContext.Domain;
using System;
using System.Threading.Tasks;

namespace workstation_backend.OfficesContext.Interface
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly RatingCommandService _ratingCommandService;
        private readonly IRatingRepository _ratingRepository;

        public RatingController(
            RatingCommandService ratingCommandService,
            IRatingRepository ratingRepository)
        {
            _ratingCommandService = ratingCommandService;
            _ratingRepository = ratingRepository;
        }

        // POST: api/rating
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRatingCommand command)
        {
            var result = await _ratingCommandService.CreateRatingAsync(command);
            return Ok(result);
        }

        // GET: api/rating/office/{officeId}
        [HttpGet("office/{officeId}")]
        public async Task<IActionResult> GetByOffice(Guid officeId)
        {
            var ratings = await _ratingRepository.GetByOfficeIdAsync(officeId);
            return Ok(ratings);
        }

        // GET: api/rating/office/{officeId}/average
        [HttpGet("office/{officeId}/average")]
        public async Task<IActionResult> GetAverage(Guid officeId)
        {
            var ratings = await _ratingRepository.GetByOfficeIdAsync(officeId);

            if (!ratings.Any())
                return Ok(0);

            var average = ratings.Average(r => r.Score);
            return Ok(Math.Round(average, 2));
        }
    }
}