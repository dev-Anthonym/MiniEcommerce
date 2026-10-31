namespace MiniEcommerce.Console.Services;

public class Catalogo
{
    private readonly List<Produto> _produtos = new();

    public void Adicionar(Produto produto)
    {
        ArgumentNullException.ThrowIfNull(produto);

        if (BuscarPorNome(produto.Nome) is not null)
            throw new InvalidOperationException($"Já existe um produto chamado '{produto.Nome}'.");

        _produtos.Add(produto);
    }

    public IReadOnlyList<Produto> Listar() => _produtos.AsReadOnly();

    public Produto? BuscarPorNome(string nome) =>
        _produtos.FirstOrDefault(p =>
            p.Nome.Equals(nome?.Trim(), StringComparison.OrdinalIgnoreCase));
}