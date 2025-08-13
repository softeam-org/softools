using FastEndpoints;
using FluentValidation;
using Softools.Projetos.Models.DTOs;

namespace Softools.Projetos.Models.Validations;

public class ProjetoRequestValidation: Validator<ProjetoRequest>
{
    public ProjetoRequestValidation()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("Nome é obrigatório")
            .MinimumLength(3)
            .WithMessage("Nome muito curto")
            .MaximumLength(100)
            .WithMessage("Nome muito longo");

        RuleFor(x => x.DataInicio)
            .NotEmpty()
            .WithMessage("Data de Inicio é Obrigatoria");
        
        RuleFor(x => x.DataFim)
            .NotEmpty()
            .WithMessage("Data de Fim é Obrigatoria");

    }
    
    
}