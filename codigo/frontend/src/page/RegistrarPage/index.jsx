import "./styles.css";
import { useContext, useState } from "react";
import { AuthContext } from "../../contexts/auth";

const RegistrarPage = () => {
   const { cadastrar } = useContext(AuthContext);
   const [nome, setNome] = useState("");
   const [cpf, setCpf] = useState("");
   const [senha, setSenha] = useState("");

   const handleRegistrar = (e) => {
      e.preventDefault();
      const dto = {
         nome: nome,
         rg: "",
         cpf: cpf,
         senha: senha,
         endereco: {
           cep: "",
           rua: "",
           numero: -1,
           bairro: "",
           cidade: "",
           estado: ""
         },
         profissao: "",
         empregador: "",
         rendimentoMensal: -1,
       };
      cadastrar(dto);
   }

   return (
      <div id="registrar">
         <h1 className="title">Cadastrar</h1>
         <form className="form">
            <div className="field">
               <label htmlFor="nome">Nome</label>
               <input type="text" name="nome" id="nome" onChange={(e) => setNome(e.target.value)} />
            </div>

            <div className="field">
               <label htmlFor="cpf">CPF</label>
               <input type="text" name="cpf" id="cpf" onChange={(e) => setCpf(e.target.value)}/>
            </div>

            <div className="field">
               <label htmlFor="senha">Senha</label>
               <input type="password" name="senha" id="senha" onChange={(e) => setSenha(e.target.value)}/>
            </div>

            <div className="actions">
               <button type="submit" onClick={handleRegistrar}>Cadastrar</button>
            </div>
         </form>
      </div>
   );
};

export default RegistrarPage;
