namespace MiniEcommerce.Console.Models;

public class Produto
{
    public string Nome { get; }
    public decimal Preco { get; }
    public int Estoque { get; private set; }

    public Produto(string nome, decimal preco, int estoque)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do produto é obrigatório.", nameof(nome));

        if (preco < 0)
            throw new ArgumentException("O preço não pode ser negativo.", nameof(preco));

        if (estoque < 0)
            throw new ArgumentException("O estoque não pode ser negativo.", nameof(estoque));

        Nome = nome.Trim();
        Preco = preco;
        Estoque = estoque;
    }

        internal void BaixarEstoque(int quantidade)
    {
        if (quantidade <= 0)
            throw new ArgumentException("A quantidade deve ser maior que zero.", nameof(quantidade));

        if (quantidade > Estoque)
            throw new InvalidOperationException($"Estoque insuficiente para '{Nome}'.");

        Estoque -= quantidade;
    }
}