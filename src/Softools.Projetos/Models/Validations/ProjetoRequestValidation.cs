using FastEndpoints;
using FluentValidation;
using Softools.Projetos.Models.DTOs;
using Softools.Projetos.Models.Enums;

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
        
        RuleFor(x => x)
            .Must(x => x.DataFim >= x.DataInicio)
            .WithMessage("A data de fim não pode ser anterior à data de início")
            .When(x => x.DataFim != default && x.DataInicio != default);

        RuleFor(x => x.LinkContrato)
            .Must((projeto, link) =>
            {
                if (projeto.Tipo == TipoProjeto.Externo) 
                    return !string.IsNullOrWhiteSpace(link) && Uri.IsWellFormedUriString(link, UriKind.Absolute);
        
                // Interno -> opcional
                if (string.IsNullOrWhiteSpace(link)) 
                    return true;

                return Uri.IsWellFormedUriString(link, UriKind.Absolute);
            })
            .WithMessage("O link do contrato é obrigatório para projetos externos e deve ser uma URL válida (http ou https).");

    }
    
    
}