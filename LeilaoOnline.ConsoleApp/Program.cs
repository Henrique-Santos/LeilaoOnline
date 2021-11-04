using LeilaoOnline.Core;
using System;

namespace LeilaoOnline.ConsoleApp
{
    class Program
    {
        static void Verifica(double esperado, double obtido)
        {
            if (esperado == obtido)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("TESTE OK");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"TESTE FALHOU: Era esperado {esperado}, mas foi obtido {obtido}");
            }
            Console.ResetColor();
        }

        static void LeilaoComVariosLances()
        {
            // Arrange -> Cenário sob teste
            var modalidade = new MaiorValor();
            var leilao = new Leilao("Van Gogh", modalidade);
            var joao = new Interessada("João", leilao);
            var maria = new Interessada("Maria", leilao);

            leilao.RecebeLance(joao, 800);
            leilao.RecebeLance(maria, 900);
            leilao.RecebeLance(joao, 1000);
            leilao.RecebeLance(maria, 990);

            // Act -> Método sob teste
            leilao.TerminaPregao();

            var valorEsperado = 0;
            var valorObtido = leilao.Ganhador.Valor;

            // Assert -> Resultado esperado
            Verifica(valorEsperado, valorObtido);
        }

        static void LeilaoComUmLances()
        {
            var modalidade = new MaiorValor();
            var leilao = new Leilao("Van Gogh", modalidade);
            var joao = new Interessada("João", leilao);

            leilao.RecebeLance(joao, 800);

            leilao.TerminaPregao();

            var valorEsperado = 800;
            var valorObtido = leilao.Ganhador.Valor;

            Verifica(valorEsperado, valorObtido);
        }

        static void Main()
        {
            LeilaoComVariosLances();
            LeilaoComUmLances();
        }
    }
}
