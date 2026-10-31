namespace MiniEcommerce.Console.Models;

public class CarrinhoDeCompras
{
    public IReadOnlyList<ItemCarrinho> Itens => throw new NotImplementedException();
    public decimal Subtotal => throw new NotImplementedException();

    public void Adicionar(Produto produto, int quantidade) => throw new NotImplementedException();
    public void Remover(string nomeProduto) => throw new NotImplementedException();
}