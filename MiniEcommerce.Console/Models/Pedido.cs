namespace MiniEcommerce.Console.Models;

public class Pedido
{
    public IReadOnlyList<ItemPedido> Itens { get; }
    public DateTime DataHora { get; }
    public decimal Total => throw new NotImplementedException();
    public int QuantidadeTotalDeItens => throw new NotImplementedException();

    private Pedido(List<ItemPedido> itens)
    {
        Itens = itens.AsReadOnly();
        DataHora = DateTime.Now;
    }

    public static Pedido Finalizar(CarrinhoDeCompras carrinho) => throw new NotImplementedException();
}