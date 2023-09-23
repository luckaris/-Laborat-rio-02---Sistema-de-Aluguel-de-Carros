export interface Client {
  id?: string;
  clienteId?: string;
  nome: string;
  rg: string;
  cpf: string;
  senha?: string;
  endereco?: string;
  profissao: string;
  empregador: string;
  rendimentoMensal: string;
  permissao?: string;
}
