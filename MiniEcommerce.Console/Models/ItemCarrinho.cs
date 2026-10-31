namespace MiniEcommerce.Console.Models;

public class ItemCarrinho
{
    public Produto Produto { get; }
    public int Quantidade { get; private set; }
    public decimal Subtotal => throw new NotImplementedException();

    public ItemCarrinho(Produto produto, int quantidade)
    {
        Produto = produto;
        Quantidade = quantidade;
    }

    internal void AumentarQuantidade(int quantidade) => throw new NotImplementedException();
}