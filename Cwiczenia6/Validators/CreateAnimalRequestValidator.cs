using Cwiczenia6.DTOs;
using FluentValidation;
namespace Cwiczenia6.Validators;

public class CreateAnimalRequestValidator : AbstractValidator<CreateAnimalRequest>
{
    public CreateAnimalRequestValidator()
    {
        RuleFor(e => e.Name).MaximumLength(10).NotNull();
        RuleFor(e => e.Area).MaximumLength(10).NotNull();
        RuleFor(e => e.Category).MaximumLength(10).NotNull();
        RuleFor(e => e.Desc).MaximumLength(10).NotNull();


    }
}