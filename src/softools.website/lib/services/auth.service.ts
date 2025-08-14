import { AuthResponse } from "../dtos/auth.dto";

if (!process.env.NEXT_PUBLIC_SERVER_DOMAIN) {
  throw new Error("NEXT_PUBLIC_SERVER_DOMAIN is not set");
}
const API_DOMAIN = `http://${process.env.NEXT_PUBLIC_SERVER_DOMAIN}`;


export function getAuthHeaders(): HeadersInit {
    const token = localStorage.getItem("token");
    const headers: Record<string, string> = {};
    if (token) {
      headers["Authorization"] = `Bearer ${token}`;
    }
    return headers;
  }

  export async function login(email: string, password: string): Promise<AuthResponse> {
    const response = await fetch(`${API_DOMAIN}/auth/login`, {
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
    const response = await fetch(`${API_DOMAIN}/auth/register`, {
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
  