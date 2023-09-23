import { useFormik } from "formik";
import { useNavigate } from "react-router-dom";
import * as Yup from "yup";
import { LoginService } from "../../services";

export const SignUp = () => {
  const navigate = useNavigate();

  const formik = useFormik({
    initialValues: {
      userName: "",
      email: "",
      password: "",
    },
    validationSchema: Yup.object({
      email: Yup.string().email("Email invalido").required("Campo obrigatório"),
      password: Yup.string().required("Campo obrigatório"),
      userName: Yup.string().required("Campo obrigatório"),
    }),
    onSubmit: async (values) => {
      try {
        await LoginService.signUp(
          values.email,
          values.password,
          values.userName
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
            value={formik.values.email}
            onChange={formik.handleChange}
            id="email"
            name="email"
            type="email"
            placeholder="Email"
            className="input w-full max-w-xs"
          />
          {formik.errors.email && formik.touched.email && (
            <label className="label">
              <span className="label-text-alt text-error">
                {formik.errors.email}
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
          <input type="submit" className="btn btn-primary" value="Sign up" />
        </form>
      </div>
    </div>
  );
};
