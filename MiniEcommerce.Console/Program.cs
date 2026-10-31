using System.Globalization;
using System.Text;
using MiniEcommerce.Console.Models;

Console.OutputEncoding = Encoding.UTF8;
var cultura = new CultureInfo("pt-BR");

var catalogo = new Catalogo();
var carrinho = new CarrinhoDeCompras();

// Produtos de exemplo para facilitar os testes
catalogo.Adicionar(new Produto("Teclado", 150.00m, 10));
catalogo.Adicionar(new Produto("Mouse", 80.00m, 15));
catalogo.Adicionar(new Produto("Monitor", 900.00m, 5));

bool executando = true;

while (executando)
{
    MostrarMenu();
    string opcao = Console.ReadLine()?.Trim() ?? "";
    Console.WriteLine();

    try
    {
        switch (opcao)
        {
            case "1": CadastrarProduto(); break;
            case "2": ListarCatalogo(); break;
            case "3": AdicionarAoCarrinho(); break;
            case "4": RemoverDoCarrinho(); break;
            case "5": VerCarrinho(); break;
            case "6": FinalizarPedido(); break;
            case "0":
                executando = false;
                Console.WriteLine("Até logo!");
                break;
            default:
                Console.WriteLine("Opção inválida.");
                break;
        }
    }
    catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
    {
        Console.WriteLine($"Erro: {ex.Message}");
    }

    if (executando)
    {
        Console.WriteLine("\nPressione ENTER para continuar...");
        Console.ReadLine();
    }
}

// ===== Telas do menu =====

void MostrarMenu()
{
    Console.Clear();
    Console.WriteLine("===== MINI E-COMMERCE =====");
    Console.WriteLine("1 - Cadastrar produto");
    Console.WriteLine("2 - Listar catálogo");
    Console.WriteLine("3 - Adicionar ao carrinho");
    Console.WriteLine("4 - Remover do carrinho");
    Console.WriteLine("5 - Ver carrinho");
    Console.WriteLine("6 - Finalizar pedido");
    Console.WriteLine("0 - Sair");
    Console.WriteLine("---------------------------");
    Console.WriteLine($"Carrinho: {carrinho.Itens.Count} item(ns) | Subtotal: {carrinho.Subtotal.ToString("C", cultura)}");
    Console.Write("Escolha uma opção: ");
}

void CadastrarProduto()
{
    string nome = LerTexto("Nome: ");
    decimal preco = LerDecimal("Preço (ex: 49,90): ");
    int estoque = LerInteiro("Quantidade em estoque: ");

    catalogo.Adicionar(new Produto(nome, preco, estoque));
    Console.WriteLine($"Produto '{nome}' cadastrado com sucesso!");
}

void ListarCatalogo()
{
    var produtos = catalogo.Listar();

    if (produtos.Count == 0)
    {
        Console.WriteLine("Nenhum produto cadastrado.");
        return;
    }

    Console.WriteLine($"{"Produto",-20} {"Preço",12} {"Estoque",8}");
    Console.WriteLine(new string('-', 42));

    foreach (var p in produtos)
        Console.WriteLine($"{p.Nome,-20} {p.Preco.ToString("C", cultura),12} {p.Estoque,8}");
}

void AdicionarAoCarrinho()
{
    ListarCatalogo();
    Console.WriteLine();

    string nome = LerTexto("Nome do produto: ");
    var produto = catalogo.BuscarPorNome(nome)
        ?? throw new InvalidOperationException($"Produto '{nome}' não encontrado.");

    int quantidade = LerInteiro("Quantidade: ");
    carrinho.Adicionar(produto, quantidade);
    Console.WriteLine($"{quantidade}x {produto.Nome} adicionado(s) ao carrinho.");
}

void RemoverDoCarrinho()
{
    VerCarrinho();
    if (carrinho.Itens.Count == 0) return;

    Console.WriteLine();
    string nome = LerTexto("Nome do produto a remover: ");
    carrinho.Remover(nome);
    Console.WriteLine("Item removido do carrinho.");
}

void VerCarrinho()
{
    if (carrinho.Itens.Count == 0)
    {
        Console.WriteLine("O carrinho está vazio.");
        return;
    }

    Console.WriteLine($"{"Produto",-20} {"Qtd",5} {"Preço",12} {"Subtotal",12}");
    Console.WriteLine(new string('-', 52));

    foreach (var item in carrinho.Itens)
        Console.WriteLine($"{item.Produto.Nome,-20} {item.Quantidade,5} {item.Produto.Preco.ToString("C", cultura),12} {item.Subtotal.ToString("C", cultura),12}");

    Console.WriteLine(new string('-', 52));
    Console.WriteLine($"{"Subtotal:",-39} {carrinho.Subtotal.ToString("C", cultura),12}");
}

void FinalizarPedido()
{
    VerCarrinho();
    if (carrinho.Itens.Count == 0) return;

    Console.WriteLine();
    string confirmacao = LerTexto("Confirmar pedido? (s/n): ");

    if (!confirmacao.Equals("s", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("Pedido cancelado. Os itens continuam no carrinho.");
        return;
    }

    var pedido = Pedido.Finalizar(carrinho);

    Console.WriteLine();
    Console.WriteLine("===== RESUMO DO PEDIDO =====");
    Console.WriteLine($"Data: {pedido.DataHora.ToString("dd/MM/yyyy HH:mm", cultura)}");

    foreach (var item in pedido.Itens)
        Console.WriteLine($"{item.Quantidade}x {item.NomeProduto} ({item.PrecoUnitario.ToString("C", cultura)}) = {item.Subtotal.ToString("C", cultura)}");

    Console.WriteLine($"Quantidade de itens: {pedido.QuantidadeTotalDeItens}");
    Console.WriteLine($"TOTAL: {pedido.Total.ToString("C", cultura)}");
    Console.WriteLine("Pedido finalizado com sucesso! Estoque atualizado.");
}

// ===== Funções de leitura com validação =====

string LerTexto(string mensagem)
{
    while (true)
    {
        Console.Write(mensagem);
        string? entrada = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(entrada))
            return entrada.Trim();

        Console.WriteLine("O valor não pode ser vazio.");
    }
}

decimal LerDecimal(string mensagem)
{
    while (true)
    {
        Console.Write(mensagem);

        if (decimal.TryParse(Console.ReadLine(), NumberStyles.Number, cultura, out decimal valor))
            return valor;

        Console.WriteLine("Valor inválido. Use vírgula para os centavos (ex: 49,90).");
    }
}

int LerInteiro(string mensagem)
{
    while (true)
    {
        Console.Write(mensagem);

        if (int.TryParse(Console.ReadLine(), out int valor))
            return valor;

        Console.WriteLine("Digite um número inteiro.");
    }
}