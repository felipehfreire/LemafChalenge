
using System;
using System.Net.Http;

namespace LemafChalenge
{
    class Program
    {
      

        static HttpClient httpClient = new HttpClient();
        static int arvoresMortas = 0;
        const string url = "http://testes.ti.lemaf.ufla.br/api/Dicionario/{0}";
        public static void Main()
        {

            Console.WriteLine("######## Avaliação Técnica Desenvolvedor .Net Sênior!");
            Console.WriteLine("######## Felipe Henrique Freire!");
            Console.WriteLine("######## Por favor informe uma palavra como argumento!");
            var palavraArgumento = Console.ReadLine();
            Console.WriteLine("Hello World!   " + palavraArgumento);

            //int indice = encontrarIndice("Acronia");
            int indice = EncontrarIndice(palavraArgumento.ToString());

            if (indice == -1)
            {
                Console.Write($"Verbete não encontrado.\n{arvoresMortas} arvores foram mortas na busca");
            }
            else
            {
                Console.Write($"{arvoresMortas} arvores foram mortas para encontrar o verbete no indice {indice} ");
            }
            var r = Console.ReadKey();

        }

        // Simple binary search algorithm 
        static int BuscaBinaria(int menorIndice,
                                    int maiorIndice, string palavra)
        {
            if (maiorIndice >= menorIndice)
            {
                int mid = menorIndice + (maiorIndice - menorIndice) / 2;

                var palavraEncontrada = BuscarPalavraPorIndice(mid);
                int resultadoComparacao = palavraEncontrada.CompareTo(palavra);
                if (resultadoComparacao == 0)
                    return mid;
                if (resultadoComparacao > 0)
                    return BuscaBinaria(menorIndice, mid - 1, palavra);

                return BuscaBinaria(mid + 1, maiorIndice, palavra);
            }

            return -1;
        }
        static int EncontrarIndice(string palavraProcurada)
        {
            int menorIndice = 0, maiorIndice = 1;
            string palavraEncontrada = BuscarPalavraPorIndice(0);

            int resultadoComparacao = palavraEncontrada.CompareTo(palavraProcurada);

            while (resultadoComparacao < 0)
            {
                menorIndice = maiorIndice;
                maiorIndice = 2 * maiorIndice;
                palavraEncontrada = BuscarPalavraPorIndice(maiorIndice);
                resultadoComparacao = palavraEncontrada.CompareTo(palavraProcurada);
                if (palavraEncontrada.Equals("{\"Message\":\"An error has occurred.\"}"))
                {
                    return -1;
                }
            }
            return BuscaBinaria(menorIndice, maiorIndice, palavraProcurada);
        }

        private static string BuscarPalavraPorIndice(int indice)
        {
            var httpResponse = httpClient.GetAsync(string.Format(url, indice)).Result;
            arvoresMortas++;
            return httpResponse.Content.ReadAsStringAsync().Result.Replace("\"", "");

        }
    }

}
