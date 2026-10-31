namespace MiniEcommerce.Console.Models;

public class Pedido
{
    public IReadOnlyList<ItemPedido> Itens { get; }
    public DateTime DataHora { get; }
    public decimal Total => Itens.Sum(i => i.Subtotal);
    public int QuantidadeTotalDeItens => Itens.Sum(i => i.Quantidade);

    private Pedido(List<ItemPedido> itens)
    {
        Itens = itens.AsReadOnly();
        DataHora = DateTime.Now;
    }

    public static Pedido Finalizar(CarrinhoDeCompras carrinho)
    {
        ArgumentNullException.ThrowIfNull(carrinho);

        if (carrinho.Itens.Count == 0)
            throw new InvalidOperationException("Não é possível finalizar um pedido com o carrinho vazio.");

        // 1º: confere o estoque de TODOS os itens antes de mexer em qualquer produto
        foreach (var item in carrinho.Itens)
        {
            if (item.Quantidade > item.Produto.Estoque)
                throw new InvalidOperationException(
                    $"Estoque insuficiente para '{item.Produto.Nome}'. Disponível: {item.Produto.Estoque}, solicitado: {item.Quantidade}.");
        }

        // 2º: só depois baixa o estoque e monta os itens do pedido
        var itensPedido = new List<ItemPedido>();
        foreach (var item in carrinho.Itens)
        {
            item.Produto.BaixarEstoque(item.Quantidade);
            itensPedido.Add(new ItemPedido(item.Produto.Nome, item.Produto.Preco, item.Quantidade));
        }

        carrinho.Limpar();
        return new Pedido(itensPedido);
    }
}