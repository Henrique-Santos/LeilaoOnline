using System.Collections.Generic;
using System.Linq;

namespace LeilaoOnline.Core
{
    public enum EstadoLeilao
    {
        LeilaoAntesDoPregao,
        LeilaoEmAndamento,
        LeilaoFinalizado,
    }

    public class Leilao
    {
        public IEnumerable<Lance> Lances => _lances;
        private IList<Lance> _lances;
        private Interessada _ultimoCliente;
        private IModalidadeAvaliacao _avaliacao;

        public Lance Ganhador { get; private set; }
        public EstadoLeilao Estado { get; private set; }
        public string Peca { get; }


        public double ValorDestino { get; }

        public Leilao(string peca, IModalidadeAvaliacao avaliacao)
        {
            _lances = new List<Lance>();
            _avaliacao = avaliacao;
            Peca = peca;
            Estado = EstadoLeilao.LeilaoAntesDoPregao;
        }

        public void RecebeLance(Interessada cliente, double valor)
        {
            if (LanceEhValido(cliente))
            {
                _lances.Add(new Lance(cliente, valor));
                _ultimoCliente = cliente;
            }
        }

        public void IniciaPregao()
        {
            Estado = EstadoLeilao.LeilaoEmAndamento;
        }

        public void TerminaPregao()
        {
            if (Estado != EstadoLeilao.LeilaoEmAndamento)
                throw new System.InvalidOperationException("Não é possível terminar o pregão sem que ele tenha começado. Para isso, utilize o método IniciaPregao().");

            Ganhador = _avaliacao.Avalia(this);

            Estado = EstadoLeilao.LeilaoFinalizado;
        }

        private bool LanceEhValido(Interessada cliente)
        {
            return (Estado == EstadoLeilao.LeilaoEmAndamento)
                && (cliente != _ultimoCliente);
        }
    }
}
