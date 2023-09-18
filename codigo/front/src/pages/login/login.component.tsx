import { useFormik } from "formik";
import { useNavigate } from "react-router-dom";
import * as Yup from "yup";

export const Login = () => {
  const navigate = useNavigate();

  const formik = useFormik({
    initialValues: {
      email: "",
      password: "",
    },
    validationSchema: Yup.object({
      email: Yup.string().email("Email invalido").required("Campo obrigatório"),
      password: Yup.string().required("Campo obrigatório"),
    }),
    onSubmit: (values) => {
      console.log("values", values);
      localStorage.setItem("token", "1");
      navigate("/");
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
          <p className="text-xl">Aluguel de carros </p>
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
          <input type="submit" className="btn btn-primary" value="login" />
        </form>
      </div>
    </div>
  );
};
