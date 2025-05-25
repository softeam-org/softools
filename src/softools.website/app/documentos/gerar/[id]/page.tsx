"use client";

import { useEffect, useState } from "react";
import { useParams } from "next/navigation";
import { fetchCampos } from "@/lib/services/template.service";


export default function GerarDocumentoPage() {
  const params = useParams();
  const idParam = params.id;
  const templateId = Number(idParam);

  const [campos, setCampos] = useState<string[]>([]);
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
          campos.forEach((campo) => (initialData[campo] = ""));
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
      const res = await fetch("/api/your-post-endpoint", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ templateId, data: formData }),
      });
      if (!res.ok) throw new Error("Submit failed");
      setSubmitStatus("Form submitted successfully!");
    } catch (err) {
      setSubmitStatus("Error submitting form.");
      console.error(err);
    }
  }

  if (isNaN(templateId)) return <p>ID inválido.</p>;

  return (
    <main className="min-h-screen p-8 bg-[var(--background)] text-[var(--foreground)] max-w-3xl mx-auto">
      <h1 className="text-4xl font-bold mb-8 border-b border-[var(--softeam4)] pb-4">
        Gerar Documento - Template #{templateId}
      </h1>

      {loadingCampos && <p>Carregando campos...</p>}

      {!loadingCampos && campos.length > 0 && (
        <form onSubmit={handleSubmit} className="space-y-4">
          {campos.map((campo) => (
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

      {!loadingCampos && campos.length === 0 && <p>Nenhum campo encontrado para este template.</p>}

      {submitStatus && (
        <p className="mt-6 text-center font-semibold text-[var(--softeam4)]">{submitStatus}</p>
      )}
    </main>
  );
}
