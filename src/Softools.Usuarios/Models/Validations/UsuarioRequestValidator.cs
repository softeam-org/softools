using FastEndpoints;
using FluentValidation;

public class UsuarioRequestValidator : Validator<UsuarioRequest>
{
    public UsuarioRequestValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("Nome é obrigatório")
            .MinimumLength(3)
            .WithMessage("Nome muito curto")
            .MaximumLength(100)
            .WithMessage("Nome muito longo");

        RuleFor(x => x.CPF)
            .NotEmpty()
            .WithMessage("CPF é obrigatório")
            .Length(11)
            .WithMessage("CPF deve ter 11 dígitos")
            .Matches(@"^\d+$")
            .WithMessage("CPF deve conter apenas números");
    }
}