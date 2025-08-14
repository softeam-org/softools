"use client";

import { useState } from "react";
import { register } from "@/lib/services/auth.service"; // ajuste o path conforme seu projeto

export default function RegisterPage() {
  const [nomeCompleto, setNomeCompleto] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setError("");
    setLoading(true);

    try {
      await register(nomeCompleto, email, password); // se precisar enviar nomeCompleto, adapte a função register
      window.location.href = "/login"; // redireciona para login após registro
    } catch {
      setError("Falha ao registrar. Verifique seus dados.");
    } finally {
      setLoading(false);
    }
  }

  return (
    <main className="min-h-screen flex flex-col justify-center items-center bg-[var(--background)] text-[var(--foreground)] p-8">
      <form
        onSubmit={handleSubmit}
        className="w-full max-w-sm bg-[var(--softeam3)] p-6 rounded-lg shadow-lg"
      >
        <h1 className="text-3xl font-bold mb-6 text-center">Registrar</h1>

        {error && (
          <p className="mb-4 text-red-500 text-center">{error}</p>
        )}

        <label className="block mb-4">
          <span className="block mb-1">Nome Completo</span>
          <input
            type="text"
            value={nomeCompleto}
            onChange={(e) => setNomeCompleto(e.target.value)}
            className="w-full p-3 rounded border border-[var(--softeam4)] bg-transparent text-white focus:outline-none focus:ring-2 focus:ring-[var(--softeam2)]"
            placeholder="Seu nome completo"
            required
          />
        </label>

        <label className="block mb-4">
          <span className="block mb-1">Email</span>
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            className="w-full p-3 rounded border border-[var(--softeam4)] bg-transparent text-white focus:outline-none focus:ring-2 focus:ring-[var(--softeam2)]"
            placeholder="seu@email.com"
            required
          />
        </label>

        <label className="block mb-6">
          <span className="block mb-1">Senha</span>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            className="w-full p-3 rounded border border-[var(--softeam4)] bg-transparent text-white focus:outline-none focus:ring-2 focus:ring-[var(--softeam2)]"
            placeholder="••••••••"
            required
          />
        </label>

        <button
          type="submit"
          disabled={loading}
          className="w-full py-3 bg-[var(--softeam2)] rounded text-white font-semibold hover:bg-[var(--softeam5)] transition disabled:opacity-50"
        >
          {loading ? "Registrando..." : "Registrar"}
        </button>
      </form>
    </main>
  );
}
