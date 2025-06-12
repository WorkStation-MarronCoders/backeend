using System;
using FluentValidation;
using workstation_backend.OfficesContext.Domain.Models.Entities;

namespace workstation_backend.OfficesContext.Domain.Models.Validator;

public class OfficeValidator : AbstractValidator<Office>
{
    public OfficeValidator()

    {

        RuleFor(office => office.Location).NotEmpty().WithMessage("The office location must be listed").MaximumLength(200).WithMessage("Location can't exceed 200 characters");

        RuleFor(x => x.Capacity)

            .GreaterThan(0).WithMessage("Capacity must be more than 0.")

            .LessThanOrEqualTo(1000).WithMessage("Capacity can't be more than 1000.");

  

        RuleFor(x => x.CostPerDay)

            .GreaterThan(0).WithMessage("Cost per day must be more than 0.")

            .LessThan(100000).WithMessage("Cost per day is too much!");

  

        RuleFor(x => x.Services)

            .NotNull().WithMessage("Define a list of Services.")

            .Must(x => x.Count > 0).WithMessage("Include at least one Service");

        RuleForEach(x => x.Services).SetValidator(new OfficeServiceValidator()); }
}
