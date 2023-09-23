/* eslint-disable @typescript-eslint/no-explicit-any */
import { Client } from ".";
import { apiClient } from "..";

export class ClientService {
  static async create(client: Client) {
    const clientData: any = client;
    clientData.rendimentoMensal = Number(clientData.rendimentoMensal);
    await apiClient.post("/api/cliente/cadastrar", clientData);
  }

  static async update(client: Client) {
    const clientData: any = client;
    clientData.rendimentoMensal = Number(client.rendimentoMensal);
    await apiClient.put(`/api/cliente/${clientData.cpf}`, clientData);
  }

  static async getByCPF(cpf: string) {
    const response = await apiClient.get(`/api/cliente/${cpf}`);
    return response.data;
  }

  static async getAll(page: number) {
    const response = await apiClient.get(`/api/cliente/todos/${page}`);
    return response.data;
  }

  static async deleteByCPF(cpf: string) {
    await apiClient.delete(`/api/cliente/${cpf}`);
  }
}
