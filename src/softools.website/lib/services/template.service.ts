import { CamposDto, TemplateDto } from "@/lib/dtos/template.dto";

export async function fetchTemplates(): Promise<TemplateDto[]> {
    try {
      const response = await fetch("http://localhost:5124/templates");
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
      const response = await fetch(`http://localhost:5124/templates/campos/${id}`);
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
    const res = await fetch("http://localhost:5124/documentos/gerar/", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ templateId, nomeDisplay, campos: formData }),
    });
  
    if (!res.ok) throw new Error("Submit failed");
  
    return await res.blob();
  }