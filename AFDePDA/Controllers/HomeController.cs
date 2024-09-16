using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

public class HomeController : Controller
{
    public ActionResult Index()
    {
        return View(); 
    }

    [HttpPost]
    public ActionResult ExecutarAutomato(string tipoAutomato, string entrada)
    {
        ViewBag.TipoAutomatoSelecionado = tipoAutomato;

        if (string.IsNullOrWhiteSpace(entrada) || !entrada.All(c => c == 'a' || c == 'b' || c == '0' || c == '1'))
        {
            ViewBag.Erro = "Entrada inválida. As entradas permitidas são 'a', 'b', '0', '1'.";
            return View("Index");
        }

        if (tipoAutomato == "AFD")
        {
            bool resultado = ExecutarAFD(entrada);
            ViewBag.Resultado = resultado ? "Aceito pelo AFD" : "Rejeitado pelo AFD";
        }
        else if (tipoAutomato == "PDA")
        {
            bool resultado = ExecutarPDA(entrada);
            ViewBag.Resultado = resultado ? "Aceito pelo PDA" : "Rejeitado pelo PDA";
        }
        else
        {
            ViewBag.Resultado = "Escolha um tipo de autômato válido";
        }

        return View("Index"); // Retorna a partial view com o resultado
    }

    // Método para executar o AFD aab*aa ou a^n^b^m, sendo n um número par. Para o computador 0 significa a e 1 b
    private bool ExecutarAFD<T>(IEnumerable<T> entrada)
    {
        Stack<T> pilha = new Stack<T>();

        foreach (T item in entrada)
        {
            // Verifica se o item é do tipo esperado e empilha se for 'a' ou 0
            if (typeof(T) == typeof(char))
            {
                if (item.Equals((T)(object)'a') || item.Equals((T)(object)'0'))
                {
                    pilha.Push(item);
                }
            }
        }

        // Aceita se o número de 'a's ou 0's for par
        return pilha.Count % 2 == 0;
    }

    // Método para executar o PDA a*b* ou a^n^b^n, sendo n >= 0. Para o computador 0 significa a e 1 b
    private bool ExecutarPDA<T>(IEnumerable<T> entrada)
    {
        Stack<T> pilha = new Stack<T>();

        foreach (T item in entrada)
        {
            // Verifica se o item é do tipo esperado e empilha se for 'a' ou 0
            if (typeof(T) == typeof(char))
            {
                if (item.Equals((T)(object)'a') || item.Equals((T)(object)'0'))
                {
                    pilha.Push(item);
                }
                else if (item.Equals((T)(object)'b') || item.Equals((T)(object)'1'))
                {
                    if (pilha.Count > 0)
                    {
                        pilha.Pop();
                    }
                    else
                    {
                        return false; // Rejeita se tentar desempilhar sem itens na pilha
                    }
                }
            }
        }

        // Aceita se a pilha estiver vazia no final
        return pilha.Count == 0;
    }
}
