using System;
using FluentValidation;
using workstation_backend.OfficesContext.Domain.Models.Commands;

namespace workstation_backend.OfficesContext.Domain.Models.Validator;

public class OfficeServiceValidator :AbstractValidator<OfficeServiceCommand>
{
public OfficeServiceValidator()

    {

         RuleFor(x => x.Name)

            .NotEmpty().WithMessage("Name of the Service is obligatory")

            .MaximumLength(100).WithMessage("The name can't exceed 100 characters");

  

        RuleFor(x => x.Description)

            .MaximumLength(250).WithMessage("The Description can't exceed 250 characters");

  

        RuleFor(x => x.Cost)

            .GreaterThanOrEqualTo(0).WithMessage("Cost can't be negative.");

    }

}
