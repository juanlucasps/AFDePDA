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

        // Verificar se a entrada contém apenas caracteres permitidos
        if (!entrada.All(c => c == 'a' || c == 'b' || c == '0' || c == '1'))
        {
            ViewBag.Erro = "Entrada inválida. As entradas permitidas são: 'a', 'b', '0', '1'.";
            return View("Index");
        }

        // Converter a entrada para o formato adequado
        IEnumerable<char> entradaCaracteres = entrada.ToCharArray();
        IEnumerable<int> entradaInteiros = entrada.Select(c => c == 'a' ? 0 : c == 'b' ? 1 : int.Parse(c.ToString()));

        if (tipoAutomato == "AFD")
        {
            bool resultado;
            if (entrada.All(c => c == 'a' || c == 'b')) // Verifica se a entrada é string
            {
                resultado = ExecutarAFD(entradaCaracteres);
            }
            else // Se não é string, deve ser inteiro
            {
                resultado = ExecutarAFD(entradaInteiros);
            }
            ViewBag.Resultado = resultado ? "Aceito pelo AFD" : "Rejeitado pelo AFD";
        }
        else if (tipoAutomato == "PDA")
        {
            bool resultado;
            if (entrada.All(c => c == 'a' || c == 'b')) // Verifica se a entrada é string
            {
                resultado = ExecutarPDA(entradaCaracteres);
            }
            else // Se não é string, deve ser inteiro
            {
                resultado = ExecutarPDA(entradaInteiros);
            }
            ViewBag.Resultado = resultado ? "Aceito pelo PDA" : "Rejeitado pelo PDA";
        }
        else
        {
            ViewBag.Resultado = "Escolha um tipo de autômato válido";
        }

        return View("Index");
    }

    // Método para executar o AFD aab*aa ou a^n^b^m, sendo n um número par. Para o computador 0 significa a e 1 b
    private bool ExecutarAFD<T>(IEnumerable<T> entrada)
    {
        Stack<T> pilha = new Stack<T>();

        foreach (T item in entrada)
        {
            // Se o item for 'a' ou 0, empilhamos
            if (item.Equals((T)Convert.ChangeType('a', typeof(T))) || item.Equals((T)Convert.ChangeType(0, typeof(T))))
            {
                pilha.Push(item);
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
            // Se o item for 'a' ou 0, empilha
            if (item.Equals((T)Convert.ChangeType('a', typeof(T))) || item.Equals((T)Convert.ChangeType(0, typeof(T))))
            {
                pilha.Push(item);
            }
            // Se o item for 'b' ou 1, desempilha
            else if (item.Equals((T)Convert.ChangeType('b', typeof(T))) || item.Equals((T)Convert.ChangeType(1, typeof(T))))
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

        // Aceita se a pilha estiver vazia no final
        return pilha.Count == 0;
    }
}
