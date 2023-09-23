import { useNavigate } from "react-router-dom";

export const Home = () => {
  const navigate = useNavigate();

  const onLogout = () => {
    localStorage.removeItem("token");
    navigate("/login");
  };

  return (
    <div>
      <button onClick={onLogout} className="btn btn-primary">
        Logout
      </button>
      <div className="tabs">
        <a className="tab tab-bordered">Clientes</a>
        <a className="tab tab-bordered tab-active">Alugueis</a>
      </div>
    </div>
  );
};
