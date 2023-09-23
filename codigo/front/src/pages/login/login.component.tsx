import { useFormik } from "formik";
import { useNavigate } from "react-router-dom";
import * as Yup from "yup";
import { LoginService } from "../../services";

export const Login = () => {
  const navigate = useNavigate();

  const formik = useFormik({
    initialValues: {
      userName: "",
      password: "",
    },
    validationSchema: Yup.object({
      userName: Yup.string().required("Campo obrigatório"),
      password: Yup.string().required("Campo obrigatório"),
    }),
    onSubmit: async (values) => {
      try {
        const token = await LoginService.login(
          values.userName,
          values.password
        );
        localStorage.setItem("token", token);
        navigate("/");
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
          <p className="text-xl">Aluguel de carros</p>
          <input
            value={formik.values.userName}
            onChange={formik.handleChange}
            id="userName"
            name="userName"
            type="userName"
            placeholder="userName"
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
        <button className="btn btn-link" onClick={() => navigate("/signUp")}>
          Sign Up
        </button>
      </div>
    </div>
  );
};
