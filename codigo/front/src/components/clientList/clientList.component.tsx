import { useQuery } from "@tanstack/react-query";
import { useState } from "react";
import { ClientForm } from "../clientForm";
import { Client, ClientService } from "../../services/client";

const modalId: string = "clientForm";

export const ClientList = () => {
  const [selectedClient, setSelectedClient] = useState<string | null>(null);

  const getClientList = async () => {
    try {
      return await ClientService.getAll(1);
    } catch (error) {
      return [];
    }
  };

  const { data, refetch } = useQuery<Client[]>({
    queryKey: ["clients"],
    queryFn: getClientList,
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
              <th>Nome</th>
              <th>cpf</th>
              <th></th>
            </tr>
          </thead>
          <tbody className="overflow-x-auto">
            {data &&
              data.length > 0 &&
              data.map(
                (client: Client) => (
                  <tr key={client.id}>
                    <td>{client.nome}</td>
                    <td>{client.cpf}</td>

                    <td>
                      <div className="flex justify-end">
                        <label
                          htmlFor={modalId}
                          className="btn btn-sm"
                          onClick={() => {
                            setSelectedClient(client.cpf!);
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
            <p className="text-gray-500">Nenhum cliente cadastrado</p>
          </div>
        )}
      </div>
      <ClientForm
        cpf={selectedClient}
        modalId={modalId}
        refetchClients={refetch}
        onClose={() => setSelectedClient(null)}
      />
    </>
  );
};
