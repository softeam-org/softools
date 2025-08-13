"use client";

import { socialSeeds } from "@/lib/dtos/socials.dto";
import Image from "next/image";

export default function CreditsPage() {
  return (
    <main className="min-h-screen p-8 bg-[var(--softeam1)] text-[var(--foreground)]">
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
        

            <h2 className="text-2xl font-semibold mb-4 border-b border-[var(--softeam4)] pb-2">
              Equipe 2025
            </h2><div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
  {socialSeeds.map(({ name, linkedin, fotoPerfil, cargo }) => (
    <div
      key={name}
      className="bg-[var(--softeam4)] border rounded-lg p-4 text-center shadow-md"
    >
      <img
        src={fotoPerfil}
        alt={name}
        className="w-20 h-20 mx-auto rounded-full mb-4"
      />
      <h3 className="text-lg font-semibold">{name}</h3>
      <p className="text-sm text-[var(--softeam2)] mb-2">{cargo}</p>
      <a
        href={linkedin}
        target="_blank"
        rel="noopener noreferrer"
        className="inline-flex items-center gap-1 text-blue-600 hover:underline"
      >
        <span className="material-icons text-base">linkedin</span>
      </a>
    </div>
  ))}
</div>



      </section>
    </main>
  );
}
