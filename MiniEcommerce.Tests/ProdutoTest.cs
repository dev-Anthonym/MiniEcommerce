using MiniEcommerce.Console.Models;

namespace MiniEcommerce.Tests;

public class ProdutoTests
{
    [Fact]
    public void CriarProduto_ComDadosValidos_DeveCriarCorretamente()
    {
        var produto = new Produto("Teclado", 150.00m, 10);

        Assert.Equal("Teclado", produto.Nome);
        Assert.Equal(150.00m, produto.Preco);
        Assert.Equal(10, produto.Estoque);
    }

    [Fact]
    public void CriarProduto_ComPrecoNegativo_DeveLancarExcecao()
    {
        Assert.Throws<ArgumentException>(() => new Produto("Mouse", -1m, 5));
    }

    [Fact]
    public void CriarProduto_ComEstoqueNegativo_DeveLancarExcecao()
    {
        Assert.Throws<ArgumentException>(() => new Produto("Mouse", 50m, -1));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void CriarProduto_ComNomeVazio_DeveLancarExcecao(string nome)
    {
        Assert.Throws<ArgumentException>(() => new Produto(nome, 50m, 5));
    }

    [Fact]
    public void CriarProduto_ComPrecoEEstoqueZero_DevePermitir()
    {
        var produto = new Produto("Brinde", 0m, 0);

        Assert.Equal(0m, produto.Preco);
        Assert.Equal(0, produto.Estoque);
    }
}