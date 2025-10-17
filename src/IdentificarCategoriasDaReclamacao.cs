using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class IdentificarCategoriasDaReclamacao
{
    public HashSet<string> ClassificarReclamacao(string reclamacao, Dictionary<string, List<string>> categorias)
    {
        string textoMinusculo = reclamacao.ToLower();
        HashSet<string> categoriasEncontradas = new HashSet<string>();

        foreach (var categoria in categorias)
        {
            string nomeCategoria = categoria.Key;
            List<string> palavrasChave = categoria.Value;

            foreach (string palavra in palavrasChave)
            {
                string pattern = @"\b" + Regex.Escape(palavra.ToLower()) + @"\b";
                if (Regex.IsMatch(textoMinusculo, pattern))
                {
                    categoriasEncontradas.Add(nomeCategoria);
                    break;
                }
            }
        }
        return categoriasEncontradas;
    }
}