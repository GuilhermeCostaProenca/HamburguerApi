using FluentValidation;
using HamburguerApi.Dtos;

namespace HamburguerApi.Validators
{
    public class HamburguerCreateValidator : AbstractValidator<HamburguerCreateDto>
    {
        public HamburguerCreateValidator()
        {
            RuleFor(x => x.Nome).NotEmpty().MaximumLength(120);
            RuleFor(x => x.Preco).GreaterThan(0).LessThanOrEqualTo(1000);
        }
    }

    public class HamburguerUpdateValidator : AbstractValidator<HamburguerUpdateDto>
    {
        public HamburguerUpdateValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Nome).NotEmpty().MaximumLength(120);
            RuleFor(x => x.Preco).GreaterThan(0).LessThanOrEqualTo(1000);
        }
    }
}
