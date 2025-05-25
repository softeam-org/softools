import { TemplateDto } from "@/lib/dtos/template.dto";

export async function fetchTemplates(): Promise<TemplateDto[]> {
    try {
      const response = await fetch("/api/templates");
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

  export async function fetchCampos(id: number): Promise<string[]> {
    return ["Foo", "Bar"];
    try {
      const response = await fetch(`https://localhost:7132/templates/campos/${id}`);
      if (!response.ok) {
        throw new Error("Failed to fetch campos");
      }
      const data = (await response.json()) as string[];
      return data;
    } catch (error) {
      console.error("Error fetching campos:", error);
      return [];
    }
  }