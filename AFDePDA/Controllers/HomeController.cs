using System.Collections.Generic;
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
        ViewBag.TipoAutomatoSelecionado = tipoAutomato; // Armazenando o valor selecionado na ViewBag

        if (tipoAutomato == "AFD")
        {
            // Chamada para executar o AFD com a entrada fornecida
            bool resultado = ExecutarAFD(entrada);
            ViewBag.Resultado = resultado ? "Aceito pelo AFD" : "Rejeitado pelo AFD";
        }
        else if (tipoAutomato == "PDA")
        {
            // Chamada para executar o PDA com a entrada fornecida
            bool resultado = ExecutarPDA(entrada);
            ViewBag.Resultado = resultado ? "Aceito pelo PDA" : "Rejeitado pelo PDA";
        }
        else
        {
            ViewBag.Resultado = "Escolha um tipo de autômato válido";
        }

        return View("Index");
    }


    // Método para executar o AFD aab*aa ou a^n^b^m, sendo n um número par
    private bool ExecutarAFD(string entrada)
    {
        int contadorA = 0;

        foreach (char c in entrada)
        {
            if (c == 'a')
                contadorA++;
        }
        // Aceita se o número de 'a's for par
        return contadorA % 2 == 0;
    }

    // Método para executar o PDA a*b* ou a^n^b^n, sendo n >= 0
    private bool ExecutarPDA(string entrada)
    {
        int contadorA = 0;

        foreach (char c in entrada)
        {
            if (c == 'a')
            {
                contadorA++; // Incrementa o contador para cada 'a' lido
            }
            else if (c == 'b')
            {
                // Se houver 'a's abertos, decrementa o contador
                if (contadorA > 0)
                {
                    contadorA--;
                }
                else
                {
                    return false; // Se não, a string é rejeitada
                }
            }
        }

        // A string é aceita se não houver 'a's abertos no final
        return contadorA == 0;
    }

}
