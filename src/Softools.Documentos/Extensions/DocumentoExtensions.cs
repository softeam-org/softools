using Softools.Documentos.Entities;
using Softools.Documentos.Models.Dtos;

namespace Softools.Documentos.Extensions;

public static class DocumentoExtensions
{
    public static DocumentoDto ToDto(this Documento documento)
    {
        var result = new DocumentoDto()
        {
            Id = documento.Id,
            Nome = documento.Nome,
            Caminho = documento.Caminho,
        };

        return result;
    }
    
}