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
    </div>
  );
};
