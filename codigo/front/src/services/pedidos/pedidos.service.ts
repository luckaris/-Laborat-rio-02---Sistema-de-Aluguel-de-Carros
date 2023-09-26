/* eslint-disable @typescript-eslint/no-explicit-any */
import { Pedido } from ".";
import { apiPedidos } from "..";

export class PedidoService {
  static async create(pedido: Pedido) {
    const pedidoData: any = pedido;
    await apiPedidos.post("/api/Pedido", pedidoData);
  }

  static async search(pedido: Pedido) {
    const pedidoData: any = pedido;
    await apiPedidos.post("/api/Pedido/pesquisar", pedidoData);
  }

  static async update(pedido: Pedido) {
    const pedidoData: any = pedido;
    await apiPedidos.put(`/api/Pedido/${pedidoData.placa}`, pedidoData);
  }

  static async getByPlaca(placa: string) {
    const response = await apiPedidos.get(`/api/Pedido/${placa}`);
    return response.data;
  }

  static async getAll(page: number) {
    const response = await apiPedidos.get(`/api/Pedido/${page}`);
    return response.data;
  }

  static async deleteByPlaca(placa: string) {
    await apiPedidos.delete(`/api/Pedido/${placa}`);
  }
}
