import "./styles.css";
import React, { useState, useContext } from "react"
import { AuthContext } from "../../contexts/auth";

const LoginPage = () => {
   const { autenticado, login } = useContext(AuthContext);

   const [cpf, setCpf] = useState("");
   const [senha, setSenha] = useState("");

   const handleSubmit = (e) => {
      e.preventDefault();
      console.log("submit", { cpf, senha });
      login(cpf, senha);
   };

   return (
      <div id="login">
         <h1 className="title">Login do Sistema</h1>
         <form className="form" onSubmit={handleSubmit}>
            <div className="field">
               <label htmlFor="cpf">CPF</label>
               <input type="number" name="cpf" id="cpf" value={cpf} onChange={(e) => setCpf(e.target.value)} />
            </div>

            <div className="field">
               <label htmlFor="senha">Senha</label>
               <input type="password" name="senha" id="senha" value={senha} onChange={(e) => setSenha(e.target.value)} />
            </div>

            <div className="actions">
               <button type="submit">Entrar</button>
            </div>
         </form>
      </div>
   );
}

export default LoginPage;
