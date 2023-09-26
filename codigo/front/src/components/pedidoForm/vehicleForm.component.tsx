import { useFormik } from "formik";
import * as Yup from "yup";
import { useRef } from "react";
import { useQuery } from "@tanstack/react-query";
import { Vehicle, VehicleService } from "../../services/vehicle";

interface IProps {
  placa: string | null;
  modalId: string;
  refetchClients: () => void;
  onClose: () => void;
}

export const VehicleForm = ({
  placa,
  modalId,
  refetchClients,
  onClose,
}: IProps) => {
  const checkboxRef = useRef<HTMLInputElement>(null);

  const formik = useFormik<Vehicle>({
    initialValues: {
      renavam: "",
      ano: 0,
      marca: "",
      modelo: "",
      placa: "",
      mensalidade: 0,
      status: ""
    },
    validationSchema: Yup.object({
      renavam: Yup.string().required("Campo obrigatório"),
      ano: Yup.number().required("Campo obrigatório"),
      marca: Yup.string().required("Campo obrigatório"),
      modelo: Yup.string().required("Campo obrigatório"),
      placa: Yup.string().required("Campo obrigatório"),
      mensalidade: Yup.number().required("Campo obrigatório"),
      status: Yup.string().required("Campo obrigatório"),
    }),
    onSubmit: async (values) => {
      try {
        if (placa) {
          await VehicleService.update(values);
        } else {
          await VehicleService.create(values);
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
      console.log(placa);
      await VehicleService.deleteByPlaca(placa!);
      formik.resetForm();
      if (checkboxRef.current) checkboxRef.current.checked = false;
      onClose();
      refetchClients();
    } catch (error) {
      console.log(error);
    }
  };

  const getSelectedVehicle = async () => {
    try {
      if (placa) {
        const vehicle = await VehicleService.getByPlaca(placa!);

        formik.setValues(vehicle);

        return vehicle;
      } else {
        return {};
      }
    } catch (error) {
      onClose();

      return {
        renavam: "",
        ano: 0,
        marca: "",
        modelo: "",
        placa: "",
        mensalidade: 0,
        status: ""
      };
    }
  };

  const { isLoading, isFetching } = useQuery({
    queryKey: ["vehicle", placa],
    queryFn: getSelectedVehicle,
    enabled: !!placa,
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
            <h2 className="text-xl">{placa ? "Editar" : "Criar"} Veículo</h2>
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
          {placa && (isLoading || isFetching) ? (
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
                    placeholder="Placa"
                    id="placa"
                    name="placa"
                    className={`input input-bordered w-full ${
                      formik.errors.placa && formik.touched.placa && "input-error"
                    }`}
                    value={formik.values.placa}
                    onChange={formik.handleChange}
                  />
                  {formik.errors.placa && formik.touched.placa && (
                    <label className="label">
                      <span className="label-text-alt text-error">
                        {formik.errors.placa}
                      </span>
                    </label>
                  )}
                </div>
                <div className="form-control w-full">
                  <input
                    type="text"
                    placeholder="Modelo"
                    id="modelo"
                    name="modelo"
                    className={`input input-bordered w-full ${
                      formik.errors.modelo &&
                      formik.touched.modelo &&
                      "input-error"
                    }`}
                    value={formik.values.modelo}
                    onChange={formik.handleChange}
                  />
                  {formik.errors.modelo && formik.touched.modelo && (
                    <label className="label">
                      <span className="label-text-alt text-error">
                        {formik.errors.modelo}
                      </span>
                    </label>
                  )}
                </div>
                <div className="form-control w-full">
                  <input
                    type="text"
                    placeholder="Marca"
                    id="marca"
                    name="marca"
                    className={`input input-bordered w-full ${
                      formik.errors.marca && formik.touched.marca && "input-error"
                    }`}
                    value={formik.values.marca}
                    onChange={formik.handleChange}
                  />
                  {formik.errors.marca && formik.touched.marca && (
                    <label className="label">
                      <span className="label-text-alt text-error">
                        {formik.errors.marca}
                      </span>
                    </label>
                  )}
                </div>
                <div className="form-control w-full">
                  <input
                    type="number"
                    placeholder="Ano"
                    id="ano"
                    name="ano"
                    className={`input input-bordered w-full ${
                      formik.errors.ano && formik.touched.ano && "input-error"
                    }`}
                    value={formik.values.ano}
                    onChange={formik.handleChange}
                  />
                  {formik.errors.ano && formik.touched.ano && (
                    <label className="label">
                      <span className="label-text-alt text-error">
                        {formik.errors.ano}
                      </span>
                    </label>
                  )}
                </div>
                <div className="form-control w-full">
                  <input
                    type="number"
                    placeholder="Mensalidade"
                    id="mensalidade"
                    name="mensalidade"
                    className={`input input-bordered w-full ${
                      formik.errors.mensalidade &&
                      formik.touched.mensalidade &&
                      "input-error"
                    }`}
                    value={formik.values.mensalidade}
                    onChange={formik.handleChange}
                  />
                  {formik.errors.mensalidade && formik.touched.mensalidade && (
                    <label className="label">
                      <span className="label-text-alt text-error">
                        {formik.errors.mensalidade}
                      </span>
                    </label>
                  )}
                </div>
                <div className="form-control w-full">
                  <input
                    type="text"
                    placeholder="Status"
                    id="status"
                    name="status"
                    className={`input input-bordered w-full ${
                      formik.errors.status &&
                      formik.touched.status &&
                      "input-error"
                    }`}
                    value={formik.values.status}
                    onChange={formik.handleChange}
                  />
                  {formik.errors.status && formik.touched.status && (
                    <label className="label">
                      <span className="label-text-alt text-error">
                        {formik.errors.status}
                      </span>
                    </label>
                  )}
                </div>
                <div className="form-control w-full">
                  <input
                    type="text"
                    placeholder="Renavam"
                    id="renavam"
                    name="renavam"
                    className={`input input-bordered w-full ${
                      formik.errors.renavam &&
                      formik.touched.renavam &&
                      "input-error"
                    }`}
                    value={formik.values.renavam}
                    onChange={formik.handleChange}
                  />
                  {formik.errors.renavam && formik.touched.renavam && (
                    <label className="label">
                      <span className="label-text-alt text-error">
                        {formik.errors.renavam}
                      </span>
                    </label>
                  )}
                </div>
              </div>
              <div className="modal-action">
                {placa && (
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
