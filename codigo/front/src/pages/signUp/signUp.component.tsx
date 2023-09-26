import { useFormik } from "formik";
import { useNavigate } from "react-router-dom";
import * as Yup from "yup";
import { LoginService } from "../../services";

export const SignUp = () => {
  const navigate = useNavigate();

  const formik = useFormik({
    initialValues: {
      userName: "",
      identifier: "",
      password: "",
      type: "",
    },
    validationSchema: Yup.object({
      identifier: Yup.string().required("Campo obrigatório"),
      password: Yup.string().required("Campo obrigatório"),
      userName: Yup.string().required("Campo obrigatório"),
      type: Yup.string().required("Campo obrigatório"),
    }),
    onSubmit: async (values) => {
      try {
        await LoginService.signUp(
          values.userName,
          values.identifier,
          values.password,
          values.type
        );
        navigate("/login");
      } catch (error) {
        console.error(error);
      }
    },
  });

  return (
    <div className="h-full flex flex-col items-center justify-center">
      <div className="card w-[350px]">
        <form
          onSubmit={(e) => {
            formik.handleSubmit(e);
          }}
          className="card-body flex flex-col gap-6 justify-center bg-base-200 shadow-xl"
        >
          <p className="text-xl">Cadastro Usuario</p>
          <input
            value={formik.values.userName}
            onChange={formik.handleChange}
            id="userName"
            name="userName"
            type="text"
            placeholder="Nome"
            className="input w-full max-w-xs"
          />
          {formik.errors.userName && formik.touched.userName && (
            <label className="label">
              <span className="label-text-alt text-error">
                {formik.errors.userName}
              </span>
            </label>
          )}
          <input
            value={formik.values.identifier}
            onChange={formik.handleChange}
            id="identifier"
            name="identifier"
            type="identifier"
            placeholder="CPF"
            className="input w-full max-w-xs"
          />
          {formik.errors.identifier && formik.touched.identifier && (
            <label className="label">
              <span className="label-text-alt text-error">
                {formik.errors.identifier}
              </span>
            </label>
          )}
          <input
            value={formik.values.password}
            onChange={formik.handleChange}
            id="password"
            name="password"
            type="password"
            placeholder="Senha"
            className="input w-full max-w-xs"
          />
          {formik.errors.password && formik.touched.password && (
            <label className="label">
              <span className="label-text-alt text-error">
                {formik.errors.password}
              </span>
            </label>
          )}
          <input
            value={formik.values.type}
            onChange={formik.handleChange}
            id="type"
            name="type"
            type="type"
            placeholder="Tipo de usuario"
            className="input w-full max-w-xs"
          />
          {formik.errors.password && formik.touched.type && (
            <label className="label">
              <span className="label-text-alt text-error">
                {formik.errors.type}
              </span>
            </label>
          )}
          <input type="submit" className="btn btn-primary" value="Sign up" />
        </form>
      </div>
    </div>
  );
};
