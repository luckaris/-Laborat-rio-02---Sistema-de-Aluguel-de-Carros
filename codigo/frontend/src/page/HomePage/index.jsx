import { useContext } from "react";
import { AuthContext } from "../../contexts/auth";

const HomePage = () => {
   const { autenticado, logout } = useContext(AuthContext);
   const handleLogout = () => {
      logout();
   };

   return (
      <>
         <h1>Home Page</h1>
         <p>{String(autenticado)}</p>
         <button onClick={handleLogout}>Logout</button>
      </>
   );
}

export default HomePage;
