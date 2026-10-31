namespace MiniEcommerce.Console.Models;

public class Produto
{
    public string Nome { get; } = string.Empty;
    public decimal Preco { get; }
    public int Estoque { get; private set; }

    public Produto(string nome, decimal preco, int estoque)
    {
        throw new NotImplementedException();
    }
}