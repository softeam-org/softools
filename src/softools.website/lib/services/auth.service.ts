import { AuthResponse } from "../dtos/auth.dto";

export function getAuthHeaders(): HeadersInit {
    const token = localStorage.getItem("token");
    const headers: Record<string, string> = {};
    if (token) {
      headers["Authorization"] = `Bearer ${token}`;
    }
    return headers;
  }

  export async function login(email: string, password: string): Promise<AuthResponse> {
    const response = await fetch("http://localhost/auth/login", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ email, password }),
    });
  
    if (!response.ok) throw new Error("Login failed");
  
    const data = await response.json();
  
    if (data.token) {
      localStorage.setItem("token", data.token);
    }
  
    return data;
  }
  
  export async function register(fullname: string, email: string, password: string): Promise<AuthResponse> {
    const response = await fetch("http://localhost/auth/register", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ email, password }),
    });
  
    if (!response.ok) throw new Error("Registration failed");
  
    const data = await response.json();
  
    if (data.token) {
      localStorage.setItem("token", data.token);
    }
  
    return data;
  }
  