import { useFormik } from "formik";
import * as Yup from "yup";
import { useRef } from "react";
import { useQuery } from "@tanstack/react-query";
import { Client, ClientService } from "../../services/client";

interface IProps {
  cpf: string | null;
  modalId: string;
  refetchClients: () => void;
  onClose: () => void;
}

export const ClientForm = ({
  cpf,
  modalId,
  refetchClients,
  onClose,
}: IProps) => {
  const checkboxRef = useRef<HTMLInputElement>(null);

  const formik = useFormik<Client>({
    initialValues: {
      nome: "",
      rg: "",
      cpf: "",
      senha: "",
      endereco: "",
      profissao: "",
      empregador: "",
      rendimentoMensal: "",
    },
    validationSchema: Yup.object({
      nome: Yup.string().required("Campo obrigatório"),
      rg: Yup.string().required("Campo obrigatório"),
      cpf: Yup.string().required("Campo obrigatório"),
      senha: cpf ? Yup.string() : Yup.string().required("Campo obrigatório"),
      endereco: Yup.string().required("Campo obrigatório"),
      profissao: Yup.string().required("Campo obrigatório"),
      empregador: Yup.string().required("Campo obrigatório"),
      rendimentoMensal: Yup.string().required("Campo obrigatório"),
    }),
    onSubmit: async (values) => {
      try {
        if (cpf) {
          await ClientService.update(values);
        } else {
          await ClientService.create(values);
        }
      } catch (error) {
        console.error(error);
      } finally {
        formik.resetForm();
        if (checkboxRef.current) checkboxRef.current.checked = false;
        onClose();
        refetchClients();
      }
    },
  });

  const deleteClient = async () => {
    try {
      console.log(cpf);
      await ClientService.deleteByCPF(cpf!);
      formik.resetForm();
      if (checkboxRef.current) checkboxRef.current.checked = false;
      onClose();
      refetchClients();
    } catch (error) {
      console.log(error);
    }
  };

  const getSelectedClient = async () => {
    try {
      if (cpf) {
        const client = await ClientService.getByCPF(cpf!);

        formik.setValues(client);

        return client;
      } else {
        return {};
      }
    } catch (error) {
      onClose();

      return {
        nome: "",
        rg: "",
        cpf: "",
        senha: "",
        endereco: "",
        profissao: "",
        empregador: "",
        rendimentoMensal: "",
      };
    }
  };

  const { isLoading, isFetching } = useQuery({
    queryKey: ["client", cpf],
    queryFn: getSelectedClient,
    enabled: !!cpf,
  });

  return (
    <>
      <input
        ref={checkboxRef}
        type="checkbox"
        id={modalId}
        className="modal-toggle"
      />
      <div className="modal">
        <div className="modal-box max-h-[700px]">
          <div className="flex justify-between items-center mb-6">
            <h2 className="text-xl">{cpf ? "Editar" : "Criar"} Cliente</h2>
            <label
              htmlFor={modalId}
              className="modal-close btn btn-ghost"
              onClick={() => {
                formik.resetForm();
                onClose();
              }}
            >
              <span>
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  width="40"
                  height="40"
                  viewBox="0 0 40 40"
                >
                  <line
                    x1="10"
                    y1="10"
                    x2="30"
                    y2="30"
                    stroke="red"
                    stroke-width="2"
                  />
                  <line
                    x1="30"
                    y1="10"
                    x2="10"
                    y2="30"
                    stroke="red"
                    stroke-width="2"
                  />
                </svg>
              </span>
            </label>
          </div>
          {cpf && (isLoading || isFetching) ? (
            <div className="flex justify-center items-center h-96">
              <div className="loader ease-linear rounded-full border-8 border-t-8 border-gray-600 h-16 w-16"></div>
            </div>
          ) : (
            <form
              onSubmit={(e) => {
                formik.handleSubmit(e);
              }}
            >
              <div className="flex flex-col gap-4 overflow-y-auto max-h-[460px] p-2">
                <div className="form-control w-full">
                  <input
                    type="text"
                    placeholder="Nome"
                    id="nome"
                    name="nome"
                    className={`input input-bordered w-full ${
                      formik.errors.nome && formik.touched.nome && "input-error"
                    }`}
                    value={formik.values.nome}
                    onChange={formik.handleChange}
                  />
                  {formik.errors.nome && formik.touched.nome && (
                    <label className="label">
                      <span className="label-text-alt text-error">
                        {formik.errors.nome}
                      </span>
                    </label>
                  )}
                </div>
                <div className="form-control w-full">
                  <input
                    type="password"
                    placeholder="Senha"
                    id="senha"
                    name="senha"
                    className={`input input-bordered w-full ${
                      formik.errors.senha &&
                      formik.touched.senha &&
                      "input-error"
                    }`}
                    value={formik.values.senha}
                    onChange={formik.handleChange}
                  />
                  {formik.errors.senha && formik.touched.senha && (
                    <label className="label">
                      <span className="label-text-alt text-error">
                        {formik.errors.senha}
                      </span>
                    </label>
                  )}
                </div>
                <div className="form-control w-full">
                  <input
                    type="text"
                    placeholder="CPF"
                    id="cpf"
                    name="cpf"
                    className={`input input-bordered w-full ${
                      formik.errors.cpf && formik.touched.cpf && "input-error"
                    }`}
                    value={formik.values.cpf}
                    onChange={formik.handleChange}
                  />
                  {formik.errors.cpf && formik.touched.cpf && (
                    <label className="label">
                      <span className="label-text-alt text-error">
                        {formik.errors.cpf}
                      </span>
                    </label>
                  )}
                </div>
                <div className="form-control w-full">
                  <input
                    type="text"
                    placeholder="RG"
                    id="rg"
                    name="rg"
                    className={`input input-bordered w-full ${
                      formik.errors.rg && formik.touched.rg && "input-error"
                    }`}
                    value={formik.values.rg}
                    onChange={formik.handleChange}
                  />
                  {formik.errors.rg && formik.touched.rg && (
                    <label className="label">
                      <span className="label-text-alt text-error">
                        {formik.errors.rg}
                      </span>
                    </label>
                  )}
                </div>
                <div className="form-control w-full">
                  <input
                    type="text"
                    placeholder="Endereço"
                    id="endereco"
                    name="endereco"
                    className={`input input-bordered w-full ${
                      formik.errors.endereco &&
                      formik.touched.endereco &&
                      "input-error"
                    }`}
                    value={formik.values.endereco}
                    onChange={formik.handleChange}
                  />
                  {formik.errors.endereco && formik.touched.endereco && (
                    <label className="label">
                      <span className="label-text-alt text-error">
                        {formik.errors.endereco}
                      </span>
                    </label>
                  )}
                </div>
                <div className="form-control w-full">
                  <input
                    type="text"
                    placeholder="Profissão"
                    id="profissao"
                    name="profissao"
                    className={`input input-bordered w-full ${
                      formik.errors.profissao &&
                      formik.touched.profissao &&
                      "input-error"
                    }`}
                    value={formik.values.profissao}
                    onChange={formik.handleChange}
                  />
                  {formik.errors.profissao && formik.touched.profissao && (
                    <label className="label">
                      <span className="label-text-alt text-error">
                        {formik.errors.profissao}
                      </span>
                    </label>
                  )}
                </div>
                <div className="form-control w-full">
                  <input
                    type="text"
                    placeholder="Empregador"
                    id="empregador"
                    name="empregador"
                    className={`input input-bordered w-full ${
                      formik.errors.empregador &&
                      formik.touched.empregador &&
                      "input-error"
                    }`}
                    value={formik.values.empregador}
                    onChange={formik.handleChange}
                  />
                  {formik.errors.empregador && formik.touched.empregador && (
                    <label className="label">
                      <span className="label-text-alt text-error">
                        {formik.errors.empregador}
                      </span>
                    </label>
                  )}
                </div>
                <div className="form-control w-full">
                  <input
                    type="text"
                    placeholder="rendimentoMensal"
                    id="rendimentoMensal"
                    name="rendimentoMensal"
                    className={`input input-bordered w-full ${
                      formik.errors.rendimentoMensal &&
                      formik.touched.rendimentoMensal &&
                      "input-error"
                    }`}
                    value={formik.values.rendimentoMensal}
                    onChange={formik.handleChange}
                  />
                  {formik.errors.rendimentoMensal &&
                    formik.touched.rendimentoMensal && (
                      <label className="label">
                        <span className="label-text-alt text-error">
                          {formik.errors.rendimentoMensal}
                        </span>
                      </label>
                    )}
                </div>
              </div>
              <div className="modal-action">
                {cpf && (
                  <button
                    className="btn btn-error"
                    type="button"
                    onClick={() => deleteClient()}
                  >
                    Excluir
                  </button>
                )}
                <label htmlFor={modalId}>
                  <button type="submit" className="btn">
                    Salvar
                  </button>
                </label>
              </div>
            </form>
          )}
        </div>
      </div>
    </>
  );
};
