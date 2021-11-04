using System;
using LeilaoOnline.Core;
using Xunit;

namespace LeilaoOnline.Testes
{
    public class LeilaoCtor
    {
        [Fact]
        public void LancaArgumentExceptionDadoValorNegativo()
        {
            var valorNegativo = -100;

            var excecao = Assert.Throws<ArgumentException>(
                () => new Lance(null, valorNegativo)    
            );
        }
    }
}
