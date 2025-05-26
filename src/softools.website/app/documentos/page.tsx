"use client";

import { useEffect, useState } from "react";
import "@/app/ui/global.css";
import type { DocumentoDto } from "@/lib/dtos/document.dto.ts";
import { fetchDocumentos } from "@/lib/services/document.service.ts";

export default function DocumentoListPage() {
  const [documentos, setDocumentos] = useState<DocumentoDto[]>([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchDocumentos()
      .then((data) => setDocumentos(data))
      .finally(() => setLoading(false));
  }, []);

  const filteredDocumentos = documentos.filter(({ nome }) =>
    nome.toLowerCase().includes(searchTerm.toLowerCase())
  );

  return (
    <main className="min-h-screen p-8 bg-[var(--background)] text-[var(--foreground)]">
      <h1 className="text-4xl font-bold mb-6 border-b border-[var(--softeam4)] pb-4 max-w-3xl mx-auto">
        Documentos
      </h1>

      <div className="max-w-3xl mx-auto mb-8">
        <input
          type="search"
          placeholder="Buscar documentos..."
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          className="w-full p-3 rounded border border-[var(--softeam4)] text-white focus:outline-none focus:ring-2 focus:ring-[var(--softeam2)] bg-transparent"
        />
      </div>

      <section className="max-w-3xl mx-auto space-y-8">
        {loading ? (
          <p className="text-center text-[var(--softeam4)]">Carregando...</p>
        ) : filteredDocumentos.length > 0 ? (
          filteredDocumentos.map(({ id, nome, caminho }) => (
            <article
              key={id}
              className="relative p-6 rounded-lg border border-[var(--softeam3)] bg-[var(--softeam3)] text-[var(--foreground)] shadow-lg"
            >
              <a
                href={caminho}
                target="_blank"
                rel="noopener noreferrer"
                className="absolute top-4 right-4 text-[var(--softeam4)] hover:text-[var(--softeam5)]"
                aria-label="Abrir Documento"
              >
                <span className="material-icons">open_in_new</span>
              </a>

              <h2 className="text-2xl font-semibold mb-2">{nome}</h2>
              <p className="text-sm text-[var(--softeam4)]">{caminho}</p>
            </article>
          ))
        ) : (
          <p className="text-center text-lg text-[var(--softeam4)]">Nenhum documento encontrado.</p>
        )}
      </section>
    </main>
  );
}
