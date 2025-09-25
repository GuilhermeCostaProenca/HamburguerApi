using FluentValidation;
using HamburguerApi.Dtos;

public class ClienteCreateValidator : AbstractValidator<ClienteCreateDto>
{
    public ClienteCreateValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(160);
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
        RuleFor(x => x.Telefone).MaximumLength(30).When(x => !string.IsNullOrWhiteSpace(x.Telefone));
    }
}
public class ClienteUpdateValidator : AbstractValidator<ClienteUpdateDto>
{
    public ClienteUpdateValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(160);
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
        RuleFor(x => x.Telefone).MaximumLength(30).When(x => !string.IsNullOrWhiteSpace(x.Telefone));
    }
}
