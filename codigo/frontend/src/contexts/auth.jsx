import { createContext, useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { api, logarUsuario, criarUsuario } from "../services/api";

export const AuthContext = createContext();

export const AuthProvider = ({children}) => {
   const navigate = useNavigate();
   const [token, setToken] = useState(null);
   const [carregando, setCarregando] = useState(true);

   useEffect(() => {
      const tokenRecuperado = localStorage.getItem("token");
      if(tokenRecuperado) {
         setToken(JSON.parse(tokenRecuperado));
      }
      setCarregando(false);
   }, []);

   const login = async (cpf, senha) => {
      const response = await logarUsuario(cpf, senha);
      const token = response.data.token;

      localStorage.setItem("token", JSON.stringify(token));
      api.defaults.headers.Authorization = `Bearer ${token}`;
      setToken({ token });
      navigate('/');
   };

   const logout = () => {
      setToken(null)
      localStorage.removeItem("token");
      api.defaults.headers.Authorization = null;
      navigate('/login');
   };

   const cadastrar = async (dto) => {
      const response = await criarUsuario(dto);
      if(response.status === 201) {
         navigate('/login');
      } else {
         return;
      }
   };

   return (
      <AuthContext.Provider value={{autenticado: !!token, token, login, logout, cadastrar, carregando}}>
         {children}
      </AuthContext.Provider>
   );
};
