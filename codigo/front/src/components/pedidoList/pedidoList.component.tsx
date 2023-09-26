import { useQuery } from "@tanstack/react-query";
import { useState } from "react";
import { Pedido, PedidoService } from "../../services/pedidos";

const modalId: string = "clientForm";

export const PedidoList = () => {
  const [selectedClient, setSelectedClient] = useState<string | null>(null);

  const getPedidoList = async () => {
    try {
      return await PedidoService.getAll(0);
    } catch (error) {
      return [];
    }
  };

  const { data, refetch } = useQuery<Pedido[]>({
    queryKey: ["clients"],
    queryFn: getPedidoList,
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
              <th>Cliente</th>
              <th>Placa</th>
              <th>Status</th>
            </tr>
          </thead>
          <tbody className="overflow-x-auto">
            {data &&
              data.length > 0 &&
              data.map(
                (pedido: Pedido) => (
                  <tr key={pedido.placaAutomovel}>
                    <td>{pedido.cpfCliente}</td>
                    <td>{pedido.placaAutomovel}</td>

                    <td>
                      <div className="flex justify-end">
                        <label
                          htmlFor={modalId}
                          className="btn btn-sm"
                          onClick={() => {
                            setSelectedClient(pedido.status!);
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
