using FluentValidation;
using WebApplication1.Models.Dtos;

namespace WebApplication1.Validation.Roll
{
    public class CreateValidator : AbstractValidator<CreateDtoRoll>
    {

        public CreateValidator()
        {
            RuleFor(r => r.Length).GreaterThan(0);
            RuleFor(r => r.Weight).GreaterThan(0);
        }
    }
}
