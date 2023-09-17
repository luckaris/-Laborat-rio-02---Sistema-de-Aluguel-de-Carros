namespace Cliente.API.Models;

public class Cliente
{
    public string Nome { get; set; } = string.Empty;
    public string Rg { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Profissao { get; set; } = string.Empty;
    public string Empregador { get; set; } = string.Empty;
    public required Endereco Endereco { get; set; }
    public required List<decimal> RendimentosMensais { get; set; }
    public required List<PedidoAluguel> Pedidos {  get; set; }
}
