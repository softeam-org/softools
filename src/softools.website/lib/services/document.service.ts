import { DocumentoDto } from "../dtos/document.dto";

export async function fetchDocumentos(): Promise<DocumentoDto[]> {
    try {
      const response = await fetch("http://localhost:5124/documentos");
      if (!response.ok) {
        throw new Error("Failed to fetch templates");
      }
      const data = (await response.json()) as DocumentoDto[];
      return data;
    } catch (error) {
      console.error("Error fetching templates:", error);
      return [];
    }
  }