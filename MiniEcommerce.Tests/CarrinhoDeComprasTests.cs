using MiniEcommerce.Console.Models;

namespace MiniEcommerce.Tests;

public class CarrinhoDeComprasTests
{
    [Fact]
    public void Adicionar_ComEstoqueSuficiente_DeveAdicionarItem()
    {
        var carrinho = new CarrinhoDeCompras();
        var produto = new Produto("Teclado", 100m, 10);

        carrinho.Adicionar(produto, 3);

        var item = Assert.Single(carrinho.Itens);
        Assert.Same(produto, item.Produto);
        Assert.Equal(3, item.Quantidade);
    }

    [Fact]
    public void Adicionar_QuantidadeIgualAoEstoque_DevePermitir()
    {
        var carrinho = new CarrinhoDeCompras();
        var produto = new Produto("Teclado", 100m, 5);

        carrinho.Adicionar(produto, 5);

        Assert.Equal(5, carrinho.Itens[0].Quantidade);
    }

    [Fact]
    public void Adicionar_QuantidadeMaiorQueEstoque_DeveLancarExcecao()
    {
        var carrinho = new CarrinhoDeCompras();
        var produto = new Produto("Teclado", 100m, 5);

        Assert.Throws<InvalidOperationException>(() => carrinho.Adicionar(produto, 6));
        Assert.Empty(carrinho.Itens);
    }

    [Fact]
    public void Adicionar_MesmoProdutoDuasVezes_DeveSomarQuantidade()
    {
        var carrinho = new CarrinhoDeCompras();
        var produto = new Produto("Teclado", 100m, 10);

        carrinho.Adicionar(produto, 2);
        carrinho.Adicionar(produto, 3);

        var item = Assert.Single(carrinho.Itens);
        Assert.Equal(5, item.Quantidade);
    }

    [Fact]
    public void Adicionar_MesmoProdutoUltrapassandoEstoque_DeveLancarExcecao()
    {
        var carrinho = new CarrinhoDeCompras();
        var produto = new Produto("Teclado", 100m, 5);
        carrinho.Adicionar(produto, 3);

        Assert.Throws<InvalidOperationException>(() => carrinho.Adicionar(produto, 3));
        Assert.Equal(3, carrinho.Itens[0].Quantidade);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Adicionar_QuantidadeInvalida_DeveLancarExcecao(int quantidade)
    {
        var carrinho = new CarrinhoDeCompras();
        var produto = new Produto("Teclado", 100m, 10);

        Assert.Throws<ArgumentException>(() => carrinho.Adicionar(produto, quantidade));
    }

    [Fact]
    public void Adicionar_ProdutoNulo_DeveLancarExcecao()
    {
        var carrinho = new CarrinhoDeCompras();

        Assert.Throws<ArgumentNullException>(() => carrinho.Adicionar(null!, 1));
    }

    [Fact]
    public void Subtotal_ComVariosItens_DeveSomarCorretamente()
    {
        var carrinho = new CarrinhoDeCompras();
        carrinho.Adicionar(new Produto("Teclado", 100m, 10), 2); // 200
        carrinho.Adicionar(new Produto("Mouse", 50m, 10), 3);    // 150

        Assert.Equal(350m, carrinho.Subtotal);
    }

    [Fact]
    public void Subtotal_CarrinhoVazio_DeveSerZero()
    {
        var carrinho = new CarrinhoDeCompras();

        Assert.Equal(0m, carrinho.Subtotal);
    }

    [Fact]
    public void Remover_ItemExistente_DeveRemoverDoCarrinho()
    {
        var carrinho = new CarrinhoDeCompras();
        carrinho.Adicionar(new Produto("Teclado", 100m, 10), 2);

        carrinho.Remover("teclado");

        Assert.Empty(carrinho.Itens);
    }

    [Fact]
    public void Remover_ItemInexistente_DeveLancarExcecao()
    {
        var carrinho = new CarrinhoDeCompras();

        Assert.Throws<InvalidOperationException>(() => carrinho.Remover("Monitor"));
    }
}