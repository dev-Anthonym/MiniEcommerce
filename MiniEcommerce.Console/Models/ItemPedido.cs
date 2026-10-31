namespace MiniEcommerce.Console.Models;

public record ItemPedido(string NomeProduto, decimal PrecoUnitario, int Quantidade)
{
    public decimal Subtotal => PrecoUnitario * Quantidade;
}