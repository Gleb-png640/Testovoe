using FluentValidation;
using WebApplication1.QueryParameters;

namespace WebApplication1.Validation.Roll
{
    public class GetStatsValidator : AbstractValidator<GetStatsQuery>
    {

        public GetStatsValidator()
        {
            RuleFor(q => q.PeriodStart).NotEmpty();
            RuleFor(q => q.PeriodEnd).NotEmpty();

            RuleFor(q => q).Must(q => q.PeriodStart <= q.PeriodEnd).WithMessage("Начало периода должно быть меньше или равно концу периода");
        }
    
    }
}
