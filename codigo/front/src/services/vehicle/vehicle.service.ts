/* eslint-disable @typescript-eslint/no-explicit-any */
import { Vehicle } from ".";
import { apiVehicle } from "..";

export class VehicleService {
  static async create(vehicle: Vehicle) {
    const vehicleData: any = vehicle;
    vehicleData.ano = Number(vehicleData.ano);
    vehicleData.mensalidade = Number(vehicleData.mensalidade);
    await apiVehicle.post("/api/automoveis", vehicleData);
  }

  static async search(vehicle: Vehicle) {
    const vehicleData: any = vehicle;
    vehicleData.ano = Number(vehicleData.ano);
    vehicleData.mensalidade = Number(vehicleData.mensalidade);
    await apiVehicle.post("/api/automoveis/pesquisar", vehicleData);
  }

  static async update(vehicle: Vehicle) {
    const vehicleData: any = vehicle;
    vehicleData.ano = Number(vehicleData.ano);
    vehicleData.mensalidade = Number(vehicleData.mensalidade);
    await apiVehicle.put(`/api/automoveis/${vehicleData.placa}`, vehicleData);
  }

  static async getByPlaca(placa: string) {
    const response = await apiVehicle.get(`/api/automoveis/${placa}`);
    return response.data;
  }

  static async getAll(page: number) {
    const response = await apiVehicle.get(`/api/automoveis/${page}`);
    return response.data;
  }

  static async deleteByPlaca(placa: string) {
    await apiVehicle.delete(`/api/automoveis/${placa}`);
  }
}
