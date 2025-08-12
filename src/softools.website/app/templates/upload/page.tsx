"use client";

import { useState } from "react";
import { uploadTemplate } from "@/lib/services/template.service.ts";

export default function UploadTemplatePage() {
  const [nome, setNome] = useState("");
  const [descricao, setDescricao] = useState("");
  const [arquivo, setArquivo] = useState<File | null>(null);
  const [status, setStatus] = useState<string | null>(null);

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();

    if (!arquivo) {
      setStatus("Selecione um arquivo.");
      return;
    }

    try {
      await uploadTemplate({ nome, descricao, arquivo });
      setStatus("Template enviado com sucesso!");
      setNome("");
      setDescricao("");
      setArquivo(null);
    } catch (err) {
      console.error(err);
      setStatus("Erro ao enviar o template.");
    }
  }

  return (
    <main className="min-h-screen p-8 bg-[var(--background)] text-[var(--foreground)] max-w-2xl mx-auto">
      <h1 className="text-4xl font-bold mb-6 border-b border-[var(--softeam4)] pb-4">Enviar Template</h1>
      <form onSubmit={handleSubmit} className="space-y-4">
        <div>
          <label className="block mb-1 font-medium">Nome</label>
          <input
            type="text"
            value={nome}
            onChange={(e) => setNome(e.target.value)}
            required
            className="w-full p-2 rounded border border-[var(--softeam4)] text-black"
          />
        </div>
        <div>
          <label className="block mb-1 font-medium">Descrição</label>
          <textarea
            value={descricao}
            onChange={(e) => setDescricao(e.target.value)}
            required
            className="w-full p-2 rounded border border-[var(--softeam4)] text-black"
          />
        </div>
        <div>
          <label className="block mb-1 font-medium">Arquivo (.docx)</label>
          <input
            type="file"
            accept=".docx"
            onChange={(e) => setArquivo(e.target.files?.[0] || null)}
            required
            className="block w-full"
          />
        </div>
        <button
          type="submit"
          className="px-6 py-2 bg-[var(--softeam2)] text-white rounded hover:bg-[var(--softeam3)] transition"
        >
          Enviar
        </button>
        {status && <p className="mt-4 font-semibold text-[var(--softeam4)]">{status}</p>}
      </form>
    </main>
  );
}
