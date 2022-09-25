using System;

namespace SerraLinhasAereasSolution.Domain.Entities
{
    public class Passagem
    {
        public int Id { get; set; }
        public string Origem { get; set; }
        public string Destino { get; set; }
        public decimal Valor { get; set; }
        public bool Reservada { get ; set;}
        public DateTime DataOrigem { get; set; }
        public DateTime DataDestino { get; set; }
        public Passagem()
        {

        }
        public Passagem(int id, string origem, string destino, decimal valor, bool reservada, DateTime dataOrigem, DateTime dataDestino)
        {
            Id = id;
            Origem = origem;
            Destino = destino;
            Valor = valor;
            Reservada = reservada;
            DataOrigem = dataOrigem;
            DataDestino = dataDestino;
        }
        public Passagem(int id)
        {
            this.Id = id;
        }
        public static bool DataValida(DateTime dataOrigem, DateTime dataDestino)
        { 
            return dataDestino > dataOrigem;
        }
    }
}
