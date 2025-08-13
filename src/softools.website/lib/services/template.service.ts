import { CamposDto, TemplateDto } from "@/lib/dtos/template.dto";
import { getAuthHeaders } from "./auth.service";

const API_DOMAIN = process.env.SERVER_DOMAIN
  ? `http://${process.env.SERVER_DOMAIN}`
  : "http://localhost";

export async function fetchTemplates(): Promise<TemplateDto[]> {
    try {
      const response = await fetch(`${API_DOMAIN}/api/documents/templates`, {
              headers: getAuthHeaders(),
            });
      if (!response.ok) {
        throw new Error("Failed to fetch templates");
      }
      const data = (await response.json()) as TemplateDto[];
      return data;
    } catch (error) {
      console.error("Error fetching templates:", error);
      return [];
    }
  }

  export async function fetchCampos(id: number): Promise<CamposDto> {
    try {
      const response = await fetch(`${API_DOMAIN}/api/documents/templates/campos/${id}`, {
        headers: getAuthHeaders(),
      });
      if (!response.ok) {
        throw new Error("Failed to fetch campos");
      }
      const data = (await response.json()) as CamposDto;
      return data;
    } catch (error) {
      console.error("Error fetching campos:", error);
      return {
        nomeTemplate: "N/A",
        campos: [],
      };
    }
  }

  export async function gerarDocumento(templateId: number, nomeDisplay: string, formData: Record<string, string>): Promise<Blob> {
    const res = await fetch(`${API_DOMAIN}/api/documents/documentos/gerar/`, {
      method: "POST",
      headers: { 
        ...getAuthHeaders()  
      },
      body: JSON.stringify({ templateId, nomeDisplay, campos: formData }),
    });
  
    if (!res.ok) throw new Error("Submit failed");
  
    return await res.blob();
  }

  export async function uploadTemplate(data: {
    nome: string;
    descricao: string;
    arquivo: File;
  }): Promise<void> {
    const formData = new FormData();
    formData.append("nome", data.nome);
    formData.append("descricao", data.descricao);
    formData.append("arquivo", data.arquivo);
  
    const response = await fetch(`${API_DOMAIN}/api/documents/documentos/upload-template`, {
      method: "POST",
      headers: getAuthHeaders(),
      body: formData,
    });
  
    if (!response.ok) {
      throw new Error("Falha ao enviar o template.");
    }
  }
  

  export async function deletarTemplate(id: number): Promise<void> {
    const response = await fetch(`${API_DOMAIN}/api/documents/templates/${id}`, {
      method: "DELETE",
      headers: getAuthHeaders(),
    });

    if (!response.ok) {
      throw new Error("Falha ao deletar o template.");
    }
  }
  