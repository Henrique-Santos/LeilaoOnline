using LeilaoOnline.Core;
using System.Linq;
using Xunit;

namespace LeilaoOnline.Testes
{
    public class LeilaoRecebeLance
    {
        [Fact]
        public void NaoPermiteLanceConsecutivoDadoMesmoCliente()
        {
            var modalidade = new MaiorValor();
            var leilao = new Leilao("Van Gogh", modalidade);
            var ana = new Interessada("Ana", leilao);
            leilao.IniciaPregao();
            leilao.RecebeLance(ana, 500);

            leilao.RecebeLance(ana, 600);

            var qtdEsperada = 1;
            var qtdObtida = leilao.Lances.Count();

            Assert.Equal(qtdEsperada, qtdObtida);
        }

        [Theory]
        [InlineData(2, new double[] { 500, 600 })]
        [InlineData(4, new double[] { 500, 600, 300, 800 })]
        public void NaoPermiteNovosLancesDadoLeilaoFinalizado(double qtdEsperada, double[] ofertas)
        {
            var modalidade = new MaiorValor();
            var leilao = new Leilao("Van Gogh", modalidade);
            var ana = new Interessada("Ana", leilao);
            var ze = new Interessada("Zé", leilao);
            leilao.IniciaPregao();
            for (int index = 0; index < ofertas.Length; index++)
            {
                var oferta = ofertas[index];
                var par = index % 2 == 0;

                if (par)
                    leilao.RecebeLance(ze, oferta);
                else
                    leilao.RecebeLance(ana, oferta);
            }
            leilao.TerminaPregao();

            leilao.RecebeLance(ana, 900);

            var valorObtido = leilao.Lances.Count();

            Assert.Equal(qtdEsperada, valorObtido);
        }

        [Theory]
        [InlineData(new double[] { 200, 300, 400, 500 })]
        [InlineData(new double[] { 200 })]
        [InlineData(new double[] { 200, 300, 400 })]
        [InlineData(new double[] { 200, 300, 400, 500, 600, 700 })]
        public void QtdPermaneceZeroDadoQuePregaoNaoFoiIniciado(double[] ofertas)
        {
            var modalidade = new MaiorValor();
            var leilao = new Leilao("Van Gogh", modalidade);
            var ana = new Interessada("Ana", leilao);

            foreach (var oferta in ofertas)
            {
                leilao.RecebeLance(ana, oferta);
            }

            var qtdEsperada = 0;
            var qtdObtida = leilao.Lances.Count();

            Assert.Equal(qtdEsperada, qtdObtida);
        }
    }
}
