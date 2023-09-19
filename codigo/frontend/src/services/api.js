import axios from 'axios';

export const api = axios.create({
   baseURL: "https://localhost:7105"
});

export const apiCliente = axios.create({
   baseURL: "https://localhost:7056"
});

export const logarUsuario = async (cpf, senha) => {
   return api.post('/api/autenticacao/logar', {cpf, senha}, {
      headers: {
         'Content-Type': 'application/json'
       }
   });
};

export const criarUsuario = async (dto) => {
   return api.post('/api/autenticacao/registrar', {
      nome: dto.nome,
      rg: dto.rg,
      cpf: dto.cpf,
      senha: dto.senha,
      endereco: {
        cep: dto.endereco.cep,
        rua: dto.endereco.rua,
        numero: dto.endereco.numero,
        bairro: dto.endereco.bairro,
        cidade: dto.endereco.cidade,
        estado: dto.endereco.estado
      },
      profissao: dto.profissao,
      empregador: dto.empregador,
      rendimentoMensal: dto.rendimentoMensal,
    }, {
      headers: {
         'Content-Type': 'application/json'
       }
   });
};

export const listarUsuarios = async (quantidade) => {
   return apiCliente.get(`/api/clientes/todos/${quantidade}`)
}
