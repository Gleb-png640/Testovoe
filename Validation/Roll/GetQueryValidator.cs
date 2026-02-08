using FluentValidation;
using WebApplication1.QueryParameters;

namespace WebApplication1.Validation.Roll
{
    public class GetQueryValidator : AbstractValidator<GetRollQuery>
    {

        public GetQueryValidator()
        {
            RuleFor(q => q.Page).GreaterThan(0);
            RuleFor(q => q.PageSize).ExclusiveBetween(0, 100);

            RuleFor(q => q)
           .Must(q => q.IdFrom <= q.IdTo)
           .When(q => q.IdFrom.HasValue && q.IdTo.HasValue);

            RuleFor(q => q)
                .Must(q => q.WeightFrom <= q.WeightTo)
                .When(q => q.WeightFrom.HasValue && q.WeightTo.HasValue);

            RuleFor(q => q)
                .Must(q => q.LengthFrom <= q.LengthTo)
                .When(q => q.LengthFrom.HasValue && q.LengthTo.HasValue);

            RuleFor(q => q)
                .Must(q => q.ReceiptedFrom <= q.ReceiptedTo)
                .When(q => q.ReceiptedFrom.HasValue && q.ReceiptedTo.HasValue);

            RuleFor(q => q)
                .Must(q => q.RemovedFrom <= q.RemovedTo)
                .When(q => q.RemovedFrom.HasValue && q.RemovedTo.HasValue);
        }
    
    }
}
