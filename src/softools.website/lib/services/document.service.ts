import { DocumentoDto } from "../dtos/document.dto";
import { getAuthHeaders } from "./auth.service";

if (!process.env.NEXT_PUBLIC_SERVER_DOMAIN) {
  throw new Error("NEXT_PUBLIC_SERVER_DOMAIN is not set");
}
const API_DOMAIN = `http://${process.env.NEXT_PUBLIC_SERVER_DOMAIN}`;


export async function fetchDocumentos(): Promise<DocumentoDto[]> {
    try {
      const response = await fetch(`${API_DOMAIN}/api/documents/documentos`, {
        headers: getAuthHeaders(),
      });
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

  export async function downloadDocumento(id: number, nome: string): Promise<void> {
    try {
      const response = await fetch(`${API_DOMAIN}/api/documents/documentos/download/${id}`, {
        headers: getAuthHeaders(),
      });
      if (!response.ok) {
        throw new Error("Failed to download documento");
      }
  
      const disposition = response.headers.get("Content-Disposition");
      let filename = `${nome}.pdf`; // fallback
  
      if (disposition && disposition.includes("filename=")) {
        const match = disposition.match(/filename="?([^"]+)"?/);
        if (match?.[1]) {
          filename = match[1];
        }
      }
  
      const blob = await response.blob();
      const url = URL.createObjectURL(blob);
      const a = document.createElement("a");
      a.href = url;
      a.download = filename;
      document.body.appendChild(a);
      a.click();
      a.remove();
      URL.revokeObjectURL(url);
    } catch (error) {
      console.error("Error downloading documento:", error);
    }
  }
  