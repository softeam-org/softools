"use client";

import { useEffect, useState } from "react";
import { useParams } from "next/navigation";
import { fetchCampos, gerarDocumento } from "@/lib/services/template.service";
import { CamposDto } from "@/lib/dtos/template.dto";


export default function GerarDocumentoPage() {
  const params = useParams();
  const idParam = params.id;
  const templateId = Number(idParam);

  const [campos, setCampos] = useState<CamposDto>();
  const [formData, setFormData] = useState<Record<string, string>>({});
  const [loadingCampos, setLoadingCampos] = useState(true);
  const [submitStatus, setSubmitStatus] = useState<string | null>(null);

  useEffect(() => {
    if (!isNaN(templateId)) {
      setLoadingCampos(true);
      fetchCampos(templateId)
        .then((campos) => {
          setCampos(campos);
          const initialData: Record<string, string> = {};
          campos.campos.forEach((campo) => (initialData[campo] = ""));
          setFormData(initialData);
        })
        .finally(() => setLoadingCampos(false));
    }
  }, [templateId]);

  function handleInputChange(campo: string, value: string) {
    setFormData((prev) => ({ ...prev, [campo]: value }));
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    try {
      const blob = await gerarDocumento(templateId, formData);
  
      const filename =
        blob.type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
          ? "documento.docx"
          : "download.pdf";
  
      const url = URL.createObjectURL(blob);
      const a = document.createElement("a");
      a.href = url;
      a.download = filename;
      document.body.appendChild(a);
      a.click();
      a.remove();
      URL.revokeObjectURL(url);
  
      setSubmitStatus("Documento gerado com sucesso!");
    } catch (err) {
      setSubmitStatus("Erro ao gerar documento.");
      console.error(err);
    }
  }
  

  if (isNaN(templateId)) return <p>ID inválido.</p>;

  return (
    <main className="min-h-screen p-8 bg-[var(--background)] text-[var(--foreground)] max-w-3xl mx-auto">
      <h1 className="text-4xl font-bold mb-8 border-b border-[var(--softeam4)] pb-4">
        Gerar Documento - {campos?.nomeTemplate}
      </h1>

      {loadingCampos && <p>Carregando campos...</p>}

      {!loadingCampos && campos!.campos.length > 0 && (
        <form onSubmit={handleSubmit} className="space-y-4">
          {campos!.campos.map((campo) => (
            <div key={campo}>
              <label htmlFor={campo} className="block mb-1 font-medium capitalize">
                {campo}
              </label>
              <input
                id={campo}
                type="text"
                value={formData[campo] || ""}
                onChange={(e) => handleInputChange(campo, e.target.value)}
                className="w-full p-2 rounded border border-[var(--softeam4)] text-black"
                required
              />
            </div>
          ))}

          <button
            type="submit"
            className="mt-4 px-6 py-2 bg-[var(--softeam2)] text-white rounded hover:bg-[var(--softeam3)] transition"
          >
            Enviar
          </button>
        </form>
      )}

      {!loadingCampos && campos!.campos.length === 0 && <p>Nenhum campo encontrado para este template.</p>}

      {submitStatus && (
        <p className="mt-6 text-center font-semibold text-[var(--softeam4)]">{submitStatus}</p>
      )}
    </main>
  );
}
