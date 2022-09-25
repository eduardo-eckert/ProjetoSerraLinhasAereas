using SerraLinhasAereasSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerraLinhasAereasSolution.Domain.Interfaces
{
    public interface IViagemRepository
    {
        void MarcarViagem(Viagem viagem);
        List<Viagem> BuscarViagensPorCliente(string cpf);
        void RemarcarViagemIda(int idViagem, DateTime dataOrigem, DateTime dataDestino);
        void RemarcarViagemIdaEVolta(int idViagem, DateTime dataOrigemIda, DateTime dataDestinoIda, DateTime dataOrigemVolta, DateTime dataDestinoVolta);
        List<Viagem> BuscarTodasViagens();
    }
}
