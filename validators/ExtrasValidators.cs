using FluentValidation;
using HamburguerApi.Dtos;

namespace HamburguerApi.Validators;

// Regras comuns
file static class CommonRules {
    public static IRuleBuilderOptions<T, string> NomeRules<T>(this IRuleBuilder<T, string> rule) =>
        rule.NotEmpty().WithMessage("Nome é obrigatório.")
            .MaximumLength(120).WithMessage("Nome deve ter no máximo 120 caracteres.");

    public static IRuleBuilderOptions<T, decimal> PrecoRules<T>(this IRuleBuilder<T, decimal> rule) =>
        rule.GreaterThanOrEqualTo(0).WithMessage("Preço não pode ser negativo.")
            .LessThanOrEqualTo(99999.99m).WithMessage("Preço muito alto.");

    public static IRuleBuilderOptions<T, int> IdRules<T>(this IRuleBuilder<T, int> rule) =>
        rule.GreaterThan(0).WithMessage("Id inválido.");
}

// ---------- BEBIDA ----------
public class BebidaCreateValidator : AbstractValidator<BebidaCreateDto>
{
    public BebidaCreateValidator()
    {
        RuleFor(x => x.Nome).NomeRules();
        RuleFor(x => x.Preco).PrecoRules();
    }
}

public class BebidaUpdateValidator : AbstractValidator<BebidaUpdateDto>
{
    public BebidaUpdateValidator()
    {
        RuleFor(x => x.Id).IdRules();
        RuleFor(x => x.Nome).NomeRules();
        RuleFor(x => x.Preco).PrecoRules();
    }
}

// ---------- ACOMPANHAMENTO ----------
public class AcompanhamentoCreateValidator : AbstractValidator<AcompanhamentoCreateDto>
{
    public AcompanhamentoCreateValidator()
    {
        RuleFor(x => x.Nome).NomeRules();
        RuleFor(x => x.Preco).PrecoRules();
    }
}

public class AcompanhamentoUpdateValidator : AbstractValidator<AcompanhamentoUpdateDto>
{
    public AcompanhamentoUpdateValidator()
    {
        RuleFor(x => x.Id).IdRules();
        RuleFor(x => x.Nome).NomeRules();
        RuleFor(x => x.Preco).PrecoRules();
    }
}

// ---------- SOBREMESA ----------
public class SobremesaCreateValidator : AbstractValidator<SobremesaCreateDto>
{
    public SobremesaCreateValidator()
    {
        RuleFor(x => x.Nome).NomeRules();
        RuleFor(x => x.Preco).PrecoRules();
    }
}

public class SobremesaUpdateValidator : AbstractValidator<SobremesaUpdateDto>
{
    public SobremesaUpdateValidator()
    {
        RuleFor(x => x.Id).IdRules();
        RuleFor(x => x.Nome).NomeRules();
        RuleFor(x => x.Preco).PrecoRules();
    }
}
