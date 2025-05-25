'use client'

import { useState } from "react";
import "@/app/ui/global.css";
import type { TemplateDto } from "@/lib/dtos/template.dto";

const mockTemplates: TemplateDto[] = [
  {
    id: 1,
    nome: "Termo de Compromisso, Responsabilidade e Confidencialidade",
    caminho: "/templates/template-a.docx",
    descricao: "Template para termos de compromisso e confidencialidade.",
  },
  {
    id: 2,
    nome: "Termo de Voluntariado",
    caminho: "/templates/template-b.docx",
    descricao: "Template para ingressar um novo membro na Softeam.",
  },
  {
    id: 3,
    nome: "Termo de Desligamento",
    caminho: "/templates/template-c.docx",
    descricao: "Template para documentar o desligamento de um membro da Softeam.",
  },
];

export default function TemplateListPage() {
  const [searchTerm, setSearchTerm] = useState("");

  // Filter templates by name or description (case insensitive)
  const filteredTemplates = mockTemplates.filter(({ nome, descricao }) =>
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
          className="w-full p-3 rounded border border-[var(--softeam4)] text-black focus:outline-none focus:ring-2 focus:ring-[var(--softeam2)]"
        />
      </div>

      <section className="max-w-3xl mx-auto space-y-8">
        {filteredTemplates.length > 0 ? (
          filteredTemplates.map(({ id, nome, caminho, descricao }) => (
            <article
              key={id}
              className="relative p-6 rounded-lg border border-[var(--softeam4)] bg-[var(--softeam5)] text-black shadow-lg"
            >
              <a
                href={caminho}
                target="_blank"
                rel="noopener noreferrer"
                className="absolute top-4 right-4 text-[var(--softeam2)] font-semibold hover:underline"
              >
                Abrir Template
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
