import { useQuery } from "@tanstack/react-query";
import { useState } from "react";
import { Vehicle, VehicleService } from "../../services/vehicle";
import { VehicleForm } from "../vehicleForm";

const modalId: string = "clientForm";

export const VehicleList = () => {
  const [selectedClient, setSelectedClient] = useState<string | null>(null);

  const getVehicleList = async () => {
    try {
      return await VehicleService.getAll(0);
    } catch (error) {
      return [];
    }
  };

  const { data, refetch } = useQuery<Vehicle[]>({
    queryKey: ["clients"],
    queryFn: getVehicleList,
    refetchOnWindowFocus: false,
  });

  return (
    <>
      <div className="flex justify-end items-center mx-12">
        <label htmlFor={modalId} className="btn">
          Criar
        </label>
      </div>
      <div className="flex flex-col h-full bg-base-200 rounded-md shadow-lg mx-12 my-6">
        <table className="table w-full">
          <thead>
            <tr>
              <th>Marca</th>
              <th>Modelo</th>
              <th>Ano</th>
            </tr>
          </thead>
          <tbody className="overflow-x-auto">
            {data &&
              data.length > 0 &&
              data.map(
                (vehicle: Vehicle) => (
                  <tr key={vehicle.marca}>
                    <td>{vehicle.modelo}</td>
                    <td>{vehicle.ano}</td>

                    <td>
                      <div className="flex justify-end">
                        <label
                          htmlFor={modalId}
                          className="btn btn-sm"
                          onClick={() => {
                            setSelectedClient(vehicle.placa!);
                          }}
                        >
                          Editar
                        </label>
                      </div>
                    </td>
                  </tr>
                ),
                []
              )}
          </tbody>
        </table>
        {(data?.length === 0 || data === undefined) && (
          <div className="flex justify-center items-center p-2">
            <p className="text-gray-500">Nenhum veículo cadastrado</p>
          </div>
        )}
      </div>
      <VehicleForm
        placa={selectedClient}
        modalId={modalId}
        refetchClients={refetch}
        onClose={() => setSelectedClient(null)}
      />
    </>
  );
};
