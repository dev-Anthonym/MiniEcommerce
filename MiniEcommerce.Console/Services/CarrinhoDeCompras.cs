namespace MiniEcommerce.Console.Services;

public class CarrinhoDeCompras
{
    private readonly List<ItemCarrinho> _itens = new();

    public IReadOnlyList<ItemCarrinho> Itens => _itens.AsReadOnly();
    public decimal Subtotal => _itens.Sum(i => i.Subtotal);

    public void Adicionar(Produto produto, int quantidade)
    {
        ArgumentNullException.ThrowIfNull(produto);

        if (quantidade <= 0)
            throw new ArgumentException("A quantidade deve ser maior que zero.", nameof(quantidade));

        var itemExistente = BuscarItem(produto.Nome);
        int quantidadeNoCarrinho = itemExistente?.Quantidade ?? 0;

        if (quantidadeNoCarrinho + quantidade > produto.Estoque)
            throw new InvalidOperationException(
                $"Estoque insuficiente para '{produto.Nome}'. Disponível: {produto.Estoque}, já no carrinho: {quantidadeNoCarrinho}.");

        if (itemExistente is null)
            _itens.Add(new ItemCarrinho(produto, quantidade));
        else
            itemExistente.AumentarQuantidade(quantidade);
    }

    public void Remover(string nomeProduto)
    {
        var item = BuscarItem(nomeProduto)
            ?? throw new InvalidOperationException($"O produto '{nomeProduto}' não está no carrinho.");

        _itens.Remove(item);
    }

    public void Limpar() => _itens.Clear();

    private ItemCarrinho? BuscarItem(string nome) =>
        _itens.FirstOrDefault(i =>
            i.Produto.Nome.Equals(nome?.Trim(), StringComparison.OrdinalIgnoreCase));
}