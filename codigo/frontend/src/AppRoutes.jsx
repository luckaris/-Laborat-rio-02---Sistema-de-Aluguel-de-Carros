import {
   BrowserRouter as Router, 
   Route, 
   Routes,
   Navigate
} from "react-router-dom";
import LoginPage from "./page/LoginPage";
import RegistrarPage from "./page/RegistrarPage";
import HomePage from "./page/HomePage";
import { AuthProvider, AuthContext } from "./contexts/auth";
import { useContext } from "react";

const AppRoutes = () => {
   const Private = ({children}) => {
      const { autenticado, carregando } = useContext(AuthContext);

      if(carregando) {
         return <div className="carregando">Carregando...</div>
      }

      if(!autenticado) {
         return <Navigate to="/login"></Navigate>;
      }
      return children;
   };
   return (
      <Router>
         <AuthProvider>
            <Routes>
               <Route exact path="/login" element={<LoginPage/>} />
               <Route exact path="/registrar" element={<RegistrarPage/>} />
               <Route exact path="/" element={<Private><HomePage/></Private>}/>
            </Routes>  
         </AuthProvider>
      </Router>
   );
};

export default AppRoutes;
