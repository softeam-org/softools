"use client";

import { useEffect, useState } from "react";
import "@/app/ui/global.css";
import type { TemplateDto } from "@/lib/dtos/template.dto";
import { fetchTemplates } from "@/lib/services/template.service.ts";

export default function TemplateListPage() {
  const [templates, setTemplates] = useState<TemplateDto[]>([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchTemplates()
      .then((data) => setTemplates(data))
      .finally(() => setLoading(false));
  }, []);

  const filteredTemplates = templates.filter(({ nome, descricao }) =>
    nome.toLowerCase().includes(searchTerm.toLowerCase()) ||
    descricao.toLowerCase().includes(searchTerm.toLowerCase())
  );

  return (
    <main className="min-h-screen p-8 bg-[var(--background)] text-[var(--foreground)]">
      <h1 className="text-4xl font-bold mb-6 border-b border-[var(--softeam4)] pb-4 max-w-3xl mx-auto">
        Templates
      </h1>

      <div className="max-w-3xl mx-auto mb-8">
        <input
          type="search"
          placeholder="Buscar templates..."
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          className="w-full p-3 rounded border border-[var(--softeam4)] text-white focus:outline-none focus:ring-2 focus:ring-[var(--softeam2)] bg-transparent"
        />
      </div>

      <section className="max-w-3xl mx-auto space-y-8">
        {loading ? (
          <p className="text-center text-[var(--softeam4)]">Carregando...</p>
        ) : filteredTemplates.length > 0 ? (
          filteredTemplates.map(({ id, nome, descricao }) => (
            <article
              key={id}
              className="relative p-6 rounded-lg border border-[var(--softeam3)] bg-[var(--softeam3)] text-[var(--foreground)] shadow-lg"
            >
              <a
                href={`/documentos/gerar/${id}`}
                target="_blank"
                rel="noopener noreferrer"
                className="absolute top-4 right-4 text-[var(--softeam4)] hover:text-[var(--softeam5)]"
                aria-label="Abrir Template"
              >
                <span className="material-icons">open_in_new</span>
              </a>

              <h2 className="text-2xl font-semibold mb-2">{nome}</h2>
              <p className="mb-4">{descricao}</p>
            </article>
          ))
        ) : (
          <p className="text-center text-lg text-[var(--softeam4)]">Nenhum template encontrado.</p>
        )}
      </section>
    </main>
  );
}
