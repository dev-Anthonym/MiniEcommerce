using MiniEcommerce.Console.Models;

namespace MiniEcommerce.Tests;

public class PedidoTests
{
    [Fact]
    public void Finalizar_CarrinhoComItens_DeveBaixarEstoque()
    {
        var teclado = new Produto("Teclado", 100m, 10);
        var mouse = new Produto("Mouse", 50m, 5);
        var carrinho = new CarrinhoDeCompras();
        carrinho.Adicionar(teclado, 3);
        carrinho.Adicionar(mouse, 2);

        Pedido.Finalizar(carrinho);

        Assert.Equal(7, teclado.Estoque);
        Assert.Equal(3, mouse.Estoque);
    }

    [Fact]
    public void Finalizar_QuantidadeIgualAoEstoque_DeveZerarEstoque()
    {
        var teclado = new Produto("Teclado", 100m, 5);
        var carrinho = new CarrinhoDeCompras();
        carrinho.Adicionar(teclado, 5);

        Pedido.Finalizar(carrinho);

        Assert.Equal(0, teclado.Estoque);
    }

    [Fact]
    public void Finalizar_DeveCalcularTotalCorretamente()
    {
        var carrinho = new CarrinhoDeCompras();
        carrinho.Adicionar(new Produto("Teclado", 100m, 10), 2); // 200
        carrinho.Adicionar(new Produto("Mouse", 50m, 10), 3);    // 150

        var pedido = Pedido.Finalizar(carrinho);

        Assert.Equal(350m, pedido.Total);
    }

    [Fact]
    public void Finalizar_DeveCalcularQuantidadeTotalDeItens()
    {
        var carrinho = new CarrinhoDeCompras();
        carrinho.Adicionar(new Produto("Teclado", 100m, 10), 2);
        carrinho.Adicionar(new Produto("Mouse", 50m, 10), 3);

        var pedido = Pedido.Finalizar(carrinho);

        Assert.Equal(5, pedido.QuantidadeTotalDeItens);
    }

    [Fact]
    public void Finalizar_DeveGuardarItensDoPedido()
    {
        var carrinho = new CarrinhoDeCompras();
        carrinho.Adicionar(new Produto("Teclado", 100m, 10), 2);

        var pedido = Pedido.Finalizar(carrinho);

        var item = Assert.Single(pedido.Itens);
        Assert.Equal("Teclado", item.NomeProduto);
        Assert.Equal(100m, item.PrecoUnitario);
        Assert.Equal(2, item.Quantidade);
        Assert.Equal(200m, item.Subtotal);
    }

    [Fact]
    public void Finalizar_DeveEsvaziarCarrinho()
    {
        var carrinho = new CarrinhoDeCompras();
        carrinho.Adicionar(new Produto("Teclado", 100m, 10), 2);

        Pedido.Finalizar(carrinho);

        Assert.Empty(carrinho.Itens);
    }

    [Fact]
    public void Finalizar_CarrinhoVazio_DeveLancarExcecao()
    {
        var carrinho = new CarrinhoDeCompras();

        Assert.Throws<InvalidOperationException>(() => Pedido.Finalizar(carrinho));
    }

    [Fact]
    public void Finalizar_CarrinhoNulo_DeveLancarExcecao()
    {
        Assert.Throws<ArgumentNullException>(() => Pedido.Finalizar(null!));
    }

    [Fact]
    public void Finalizar_EstoqueCompradoPorOutroCliente_DeveLancarExcecaoSemAlterarEstoque()
    {
        var teclado = new Produto("Teclado", 100m, 5);
        var carrinho1 = new CarrinhoDeCompras();
        var carrinho2 = new CarrinhoDeCompras();
        carrinho1.Adicionar(teclado, 3);
        carrinho2.Adicionar(teclado, 3);

        Pedido.Finalizar(carrinho1); // sobram 2

        Assert.Throws<InvalidOperationException>(() => Pedido.Finalizar(carrinho2));
        Assert.Equal(2, teclado.Estoque);
    }

    [Fact]
    public void Finalizar_ComUmItemSemEstoque_NaoDeveBaixarNenhumProduto()
    {
        var teclado = new Produto("Teclado", 100m, 10);
        var mouse = new Produto("Mouse", 50m, 5);

        var carrinho1 = new CarrinhoDeCompras();
        carrinho1.Adicionar(teclado, 2);
        carrinho1.Adicionar(mouse, 3);

        var carrinho2 = new CarrinhoDeCompras();
        carrinho2.Adicionar(mouse, 4);
        Pedido.Finalizar(carrinho2); // sobra 1 mouse

        Assert.Throws<InvalidOperationException>(() => Pedido.Finalizar(carrinho1));
        Assert.Equal(10, teclado.Estoque); // teclado não pode ter sido baixado
        Assert.Equal(1, mouse.Estoque);
    }
}