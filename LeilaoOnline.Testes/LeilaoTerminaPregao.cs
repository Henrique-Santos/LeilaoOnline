using LeilaoOnline.Core;
using Xunit;

namespace LeilaoOnline.Testes
{
    public class LeilaoTerminaPregao
    {
        [Fact]
        public void LancaInvalidOperationExceptionDadoPregaoNaoIniciado()
        {
            var modalidade = new MaiorValor();
            var leilao = new Leilao("Van Gogh", modalidade);

            var excecao = Assert.Throws<System.InvalidOperationException>(
                () => leilao.TerminaPregao()
            );

            var msgEsperada = "Não é possível terminar o pregão sem que ele tenha começado. Para isso, utilize o método IniciaPregao().";

            Assert.Equal(msgEsperada, excecao.Message);
        }

        [Fact]
        public void RetornaZeroDadoLeilaoSemLance()
        {
            var modalidade = new MaiorValor();
            var leilao = new Leilao("Van Gogh", modalidade);
            leilao.IniciaPregao();

            leilao.TerminaPregao();

            var valorEsperado = 0;
            var valorObtido = leilao.Ganhador.Valor;

            Assert.Equal(valorEsperado, valorObtido);
        }

        [Theory]
        [InlineData(1200, new double[] { 800, 900, 1000, 1200 })]
        [InlineData(1000, new double[] { 800, 1000, 900 })]
        [InlineData(800, new double[] { 800 })]
        public void RetornaMaiorValorDadoLeilaoComPeloMenosUmLance(double valorEsperado, double[] ofertas)
        {
            var modalidade = new MaiorValor();
            var leilao = new Leilao("Van Gogh", modalidade);
            var joao = new Interessada("João", leilao);
            var ana = new Interessada("Ana", leilao);
            leilao.IniciaPregao();
            for (int index = 0; index < ofertas.Length; index++)
            {
                var oferta = ofertas[index];
                var par = index % 2 == 0;

                if (par)
                    leilao.RecebeLance(joao, oferta);
                else
                    leilao.RecebeLance(ana, oferta);
            }

            leilao.TerminaPregao();

            var valorObtido = leilao.Ganhador.Valor;

            Assert.Equal(valorEsperado, valorObtido);
        }

        [Theory]
        [InlineData(1200, 1250, new double[] { 800, 1150, 1400, 1250 })]
        public void RetornaValorSuperiorMaisProximoDadoLeilaoNessaModalidade(
            double valorDestino, 
            double valorEsperado,
            double[] ofertas
        )
        {
            var modalidade = new OfertaSuperiorMaisProxima(valorDestino);
            var leilao = new Leilao("Van Gogh", modalidade);
            var ana = new Interessada("Ana", leilao);
            var joao = new Interessada("João", leilao);
            leilao.IniciaPregao();
            for (int index = 0; index < ofertas.Length; index++)
            {
                var oferta = ofertas[index];
                var par = index % 2 == 0;

                if (par)
                    leilao.RecebeLance(joao, oferta);
                else
                    leilao.RecebeLance(ana, oferta);
            }

            leilao.TerminaPregao();

            Assert.Equal(valorEsperado, leilao.Ganhador.Valor);
        }
    }
}
