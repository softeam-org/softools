"use client";

import { usePathname, useRouter } from "next/navigation";
import { useEffect } from "react";

const WHITELISTED_PATHS = ["/login", "/register", "/forgot-password"];

function isTokenExpired(token: string): boolean {
  try {
    const payloadBase64 = token.split(".")[1];
    const payloadJson = atob(payloadBase64);
    const payload = JSON.parse(payloadJson);

    if (!payload.exp) return true;

    const now = Math.floor(Date.now() / 1000);
    return payload.exp < now;
  } catch {
    return true;
  }
}

export default function AuthGuard({ children }: { children: React.ReactNode }) {
  const router = useRouter();
  const pathname = usePathname();

  useEffect(() => {
    const token = localStorage.getItem("token");

    if (WHITELISTED_PATHS.includes(pathname)) return;

    if (!token || isTokenExpired(token)) {
      localStorage.removeItem("token"); // cleanup if expired
      router.replace("/login");
    }
  }, [pathname, router]);

  // Prevent flash of protected content
  const token = typeof window !== "undefined" ? localStorage.getItem("token") : null;
  if ((!token || isTokenExpired(token)) && !WHITELISTED_PATHS.includes(pathname)) {
    return null;
  }

  return <>{children}</>;
}
