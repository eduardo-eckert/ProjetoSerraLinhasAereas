using System;

namespace SerraLinhasAereasSolution.Domain.Entities
{
    public class Viagem
    {
        public int Id { get; set; }
        public string CodigoReserva { get; set; }
        public DateTime DataCompra { get { return DateTime.Now; } }
        public decimal ValorTotal => CalculaTotal();
        public Cliente Cliente { get; set; }
        public bool SomenteIda { get; set; }
        public Passagem PassagemIda { get; set; }
        #nullable enable
        public Passagem? PassagemVolta { get; set; }
        public string ResumoDaViagem => ViagemResumida();

        public Viagem()
        {

        }
        public Viagem(int id, string codigoReserva, Cliente cliente, bool somenteIda, Passagem passagemIda, Passagem passagemVolta)
        {
            Id = id;
            CodigoReserva = codigoReserva;
            Cliente = cliente;
            SomenteIda = somenteIda;
            PassagemIda = passagemIda;
            PassagemVolta = passagemVolta;
        }
        #nullable disable
        private string ViagemResumida()
        {
            if (SomenteIda)
                return $"Seu voo de {PassagemIda.Origem} a {PassagemIda.Destino} será dia {PassagemIda.DataOrigem.ToShortDateString()} às {PassagemIda.DataOrigem.ToShortTimeString()}h";
            else
            return $"Seu voo de ida {PassagemIda.Origem} a {PassagemIda.Destino} será dia {PassagemIda.DataOrigem.ToShortDateString()} às {PassagemIda.DataOrigem.ToShortTimeString()}h " +
                    $"e seu voo de volta de {PassagemVolta.Origem} a {PassagemVolta.Destino} será dia {PassagemVolta.DataOrigem.ToShortDateString()} às {PassagemVolta.DataOrigem.ToShortTimeString()}h";
        }
        public decimal CalculaTotal()
        {
            return SomenteIda == true ? PassagemIda.Valor : PassagemIda.Valor + PassagemVolta.Valor;
        }
        public static bool DataViagemValida(DateTime dataOrigemIda, DateTime dataOrigemVolta)
        {
            return dataOrigemIda < dataOrigemVolta;
        }
        public bool CodigoValido(string codigoReserva)
        {
            return codigoReserva.Length == 6;
        }

    }
}
