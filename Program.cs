using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Consumir_WebApi2_Produtos
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync().Wait();
            Console.ReadKey();
        }

        static async Task RunAsync()
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("http://localhost:53557/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/produtos/3");
                if (response.IsSuccessStatusCode)
                {  //GET
                    Produto produto = await response.Content.ReadAsAsync<Produto>();
                    Console.WriteLine("{0}\tR${1}\t{2}", produto.Nome, produto.Preco, produto.Categoria);
                    Console.WriteLine("Produto acessado e exibido.  Tecle algo para incluir um novo produto.");
                    Console.ReadKey();
                }
                //POST
                var cha = new Produto() { Nome = "Chá Verde", Preco = 1.50M, Categoria = "Bebidas" };
                response = await client.PostAsJsonAsync("api/produtos", cha);
                Console.WriteLine("Produto cha verde incluído. Tecle algo para atualizar o preço do produto.");
                Console.ReadKey();

                if (response.IsSuccessStatusCode)
                {   //PUT
                    Uri chaUrl = response.Headers.Location;
                    cha.Preco = 2.55M;   // atualiza o preco do produto
                    response = await client.PutAsJsonAsync(chaUrl, cha);
                    Console.WriteLine("Produto preço do atualizado. Tecle algo para excluir o produto");
                    Console.ReadKey();
                    //DELETE
                    response = await client.DeleteAsync(chaUrl);
                    Console.WriteLine("Produto deletado");
                    Console.ReadKey();
                }
            }
        }
    }
}
