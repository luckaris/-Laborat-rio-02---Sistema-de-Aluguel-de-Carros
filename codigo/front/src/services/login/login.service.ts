import { apiAuth } from "..";

export class LoginService {
  static async login(userName: string, password: string) {
    const response = await apiAuth.post("/login", {
      nomeDeUsuario: userName,
      senha: password,
    });
    return response.data;
  }
  static async signUp(email: string, password: string, userName: string) {
    const response = await apiAuth.post("/register", {
      nomeDeUsuario: userName,
      email,
      senha: password,
    });
    return response.data;
  }
}