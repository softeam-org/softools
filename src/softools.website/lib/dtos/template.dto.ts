export interface TemplateDto {
    id: number;
    nome: string;
    caminho: string;
    descricao: string;
}

export interface CamposDto {
    nomeTemplate: string;
    campos: string[];
}