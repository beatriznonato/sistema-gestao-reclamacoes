using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        var identificador = new IdentificarCategoriasDaReclamacao();
        string reclamacao = "Estou com problemas para acessar minha conta e o aplicativo está travando muito.";
        var categorias = new Dictionary<string, List<string>>
        {
            { "imobiliário", new List<string> { "credito imobiliario", "casa", "apartamento" } },
            { "seguros", new List<string> { "resgate", "capitalizacao", "socorro" } },
            { "cobrança", new List<string> { "fatura", "cobrança", "valor", "indevido" } },
            { "acesso", new List<string> { "acessar", "login", "senha" } },
            { "aplicativo", new List<string> { "app", "aplicativo", "travando", "erro" } },
            { "fraude", new List<string> { "fatura", "nao reconhece divida", "fraude" } }
        };
        HashSet<string> resultado = identificador.ClassificarReclamacao(reclamacao, categorias);
        Console.WriteLine("Categorias encontradas:");
        foreach (string cat in resultado)
        {
            Console.WriteLine($"- {cat}");
        }
    }
}