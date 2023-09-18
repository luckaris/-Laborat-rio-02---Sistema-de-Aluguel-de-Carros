import api from "..";

export class LoginService {
  static async login(email: string, password: string) {
    const response = await api.post("/login", {
      email,
      password,
    });
    return response.data;
  }
}
