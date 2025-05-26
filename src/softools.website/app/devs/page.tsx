"use client";

import { credits } from "@/lib/data/credits";
import Image from "next/image";

export default function CreditsPage() {
  return (
    <main className="min-h-screen p-8 bg-[var(--background)] text-[var(--foreground)]">
      <h1 className="text-4xl font-bold mb-6 border-b border-[var(--softeam4)] pb-4 max-w-3xl mx-auto">
        Créditos
      </h1>
      <p className="text-center text-[var(--softeam4)] max-w-2xl mx-auto mb-8">
        <Image src={"/softeam.png"} alt="SofTeam Logo" width={2000} height={650} className="mx-auto mb-4" />
        <br />
        <i>A todos que dedicaram seu tempo, coração e talento: a SofTeam será para sempre grata por cada contribuição — 
  seja uma ideia valiosa, uma conversa inspiradora, um bug resolvido ou uma paçoca dividida.  
  Cada gesto, cada detalhe, cada linha de código somou para transformar sonhos em realidades.  
        <br />
        <br />
        Com Carinho, Café e Paçoca</i> ☕🥜💙
       </p>


      <section className="max-w-3xl mx-auto space-y-12">
        {credits.map(({ tool, contributors }) => (
          <div key={tool}>
            <h2 className="text-2xl font-semibold mb-4 border-b border-[var(--softeam4)] pb-2">
              {tool}
            </h2>
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
              {contributors.map(({ name, linkedin }) => (
                <a
                  key={name}
                  href={linkedin}
                  target="_blank"
                  rel="noopener noreferrer"
                  className="flex items-center p-4 border border-[var(--softeam3)] rounded-lg bg-[var(--softeam3)] hover:bg-[var(--softeam4)] transition"
                >
                  <div>
                    <p className="text-lg font-medium">{name}</p>
                    <p className="text-sm text-[var(--softeam5)]">LinkedIn</p>
                  </div>
                </a>
              ))}
            </div>
          </div>
        ))}

<h2 className="text-2xl font-semibold mb-4 border-b border-[var(--softeam4)] pb-2">
              E todos os outros que fizeram parte dessa jornada, obrigado 💙
            </h2>
      </section>
    </main>
  );
}
