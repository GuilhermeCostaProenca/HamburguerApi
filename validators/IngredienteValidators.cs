using FluentValidation;
using HamburguerApi.Dtos;

public class IngredienteCreateValidator : AbstractValidator<IngredienteCreateDto>
{
    public IngredienteCreateValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(120);
        RuleFor(x => x.Custo).GreaterThanOrEqualTo(0).LessThanOrEqualTo(1000);
    }
}
public class IngredienteUpdateValidator : AbstractValidator<IngredienteUpdateDto>
{
    public IngredienteUpdateValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(120);
        RuleFor(x => x.Custo).GreaterThanOrEqualTo(0).LessThanOrEqualTo(1000);
    }
}
