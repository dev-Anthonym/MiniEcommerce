using MiniEcommerce.Console.Models;

namespace MiniEcommerce.Tests;

public class CatalogoTests
{
    [Fact]
    public void Adicionar_ProdutoValido_DeveAparecerNaListagem()
    {
        var catalogo = new Catalogo();
        var produto = new Produto("Monitor", 900m, 3);

        catalogo.Adicionar(produto);

        Assert.Contains(produto, catalogo.Listar());
    }

    [Fact]
    public void Adicionar_ProdutoNulo_DeveLancarExcecao()
    {
        var catalogo = new Catalogo();

        Assert.Throws<ArgumentNullException>(() => catalogo.Adicionar(null!));
    }

    [Fact]
    public void Adicionar_ProdutoComNomeDuplicado_DeveLancarExcecao()
    {
        var catalogo = new Catalogo();
        catalogo.Adicionar(new Produto("Monitor", 900m, 3));

        Assert.Throws<InvalidOperationException>(
            () => catalogo.Adicionar(new Produto("monitor", 800m, 2)));
    }

    [Fact]
    public void BuscarPorNome_ProdutoExistente_DeveRetornarProduto()
    {
        var catalogo = new Catalogo();
        var produto = new Produto("Monitor", 900m, 3);
        catalogo.Adicionar(produto);

        Assert.Same(produto, catalogo.BuscarPorNome("MONITOR"));
    }

    [Fact]
    public void BuscarPorNome_ProdutoInexistente_DeveRetornarNulo()
    {
        var catalogo = new Catalogo();

        Assert.Null(catalogo.BuscarPorNome("Cadeira"));
    }
}