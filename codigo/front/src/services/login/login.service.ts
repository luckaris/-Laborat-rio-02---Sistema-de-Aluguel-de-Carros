import { apiAuth } from "..";

export class LoginService {
  static async login(identifier: string, password: string) {
    const response = await apiAuth.post("/api/autenticacao/logar", {
      identificador: identifier,
      senha: password,
    });
    return response.data.token;
  }
  static async signUp(email: string, password: string, identifier: string) {
    const response = await apiAuth.post("/api/autenticacao/registrar", {
      nomeDeUsuario: identifier,
      email,
      senha: password,
    });
    return response.data;
  }
}
