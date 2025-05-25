using Softools.Documentos.Entities;
using Softools.Documentos.Models.Dtos;

namespace Softools.Documentos.Extensions;

public static class TemplateExtensions
{
    public static TemplateDocumentoDto ToDto(this TemplateDocumento template)
    {
        var dto = new TemplateDocumentoDto()
        {
            Id = template.Id,
            Nome = template.Nome,
            Caminho = template.Caminho,
            Descricao = template.Descricao,
        };

        return dto;
    }
}