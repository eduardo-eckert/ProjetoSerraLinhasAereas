using SerraLinhasAereasSolution.Domain.Entities;
using SerraLinhasAereasSolution.Domain.Interfaces;
using SerraLinhasAereasSolution.Infra.Data.DAO;
using System;
using System.Collections.Generic;


namespace SerraLinhasAereasSolution.Infra.Data.Repository
{
    public class ViagemRepository : IViagemRepository
    {
        private readonly ViagemDAO _viagemDAO;
        private readonly PassagemDAO _passagemDAO;
        private readonly ClienteDAO _clienteDAO;
        public ViagemRepository()
        {
            _viagemDAO = new ViagemDAO();
            _passagemDAO = new PassagemDAO();
            _clienteDAO = new ClienteDAO();
        }
        public void MarcarViagem(Viagem viagem)
        {
            viagem.Cliente = _clienteDAO.BuscarClientePorCpf(viagem.Cliente.Cpf);
            viagem.PassagemIda = _passagemDAO.BuscarPassagemPorId(viagem.PassagemIda.Id);
            viagem.PassagemVolta = _passagemDAO.BuscarPassagemPorId(viagem.PassagemVolta.Id);
            var listaViagens = _viagemDAO.BuscarTodasViagens();
            var viagemEncontrada = listaViagens.Find(v => v.CodigoReserva == viagem.CodigoReserva);
            if (!viagem.CodigoValido(viagem.CodigoReserva))
                throw new Exception($"O código de reserva informado não é válido!");
            else if (viagemEncontrada != null)
                throw new Exception($"O código de reserva já foi utilizado em outra viagem!");
            else if (viagem.Cliente == null)
                throw new Exception($"O cliente não está cadastrado no sistema!");
            else if (viagem.PassagemIda.Reservada)
                throw new Exception($"A passagem selecionada para ida já está reservada para outra viagem!");
            else if (viagem.PassagemVolta != null)
                if (viagem.PassagemVolta.Reservada)
                    throw new Exception($"A passagem selecionada para volta já está reservada para outra viagem!");
            if (viagem.PassagemVolta != null)
                if (!Viagem.DataViagemValida(viagem.PassagemIda.DataOrigem, viagem.PassagemVolta.DataOrigem))
                    throw new Exception($"A data de volta não pode ser anterior à data de ida!");

            viagem.PassagemIda.Reservada = true;
            _passagemDAO.ReservarPassagem(viagem.PassagemIda);
            _viagemDAO.MarcarViagem(viagem);
            if (viagem.PassagemVolta != null)
            {
                viagem.PassagemVolta.Reservada = true;
                _passagemDAO.ReservarPassagem(viagem.PassagemVolta);
            }
        }
        public List<Viagem> BuscarViagensPorCliente(string cpf)
        {
            var clienteBuscado = _clienteDAO.BuscarClientePorCpf(cpf);
            if (clienteBuscado == null)
                throw new Exception("O cliente buscado não está cadastrado no sistema!");
            else
            {
                var listaViagens = _viagemDAO.BuscaViagensPorCliente(clienteBuscado);
                if (listaViagens.Count == 0)
                    throw new Exception($"O cliente com CPF {clienteBuscado.Cpf} não possui nenhuma viagem cadastrada!");
                else
                    return listaViagens;
            }
        }
        public void RemarcarViagemIda(int idViagem, DateTime dataOrigem, DateTime dataDestino)
        {
            var viagemRemarcada = _viagemDAO.BuscaViagensPorId(idViagem);
            if (viagemRemarcada == null)
                throw new Exception("A viagem solicitada para remarcar não consta no sistema!");

            var passagemValida = Passagem.DataValida(dataOrigem, dataDestino);
            if(passagemValida)
                _viagemDAO.RemarcarViagemIda(viagemRemarcada.Id, dataOrigem, dataDestino);
            else
                throw new Exception($"A data de volta não pode ser anterior à data de ida!");
        }
        public void RemarcarViagemIdaEVolta(int idViagem, DateTime dataOrigemIda, DateTime dataDestinoIda, DateTime dataOrigemVolta, DateTime dataDestinoVolta)
        {
            var viagemRemarcada = _viagemDAO.BuscaViagensPorId(idViagem);
            if (viagemRemarcada == null)
                throw new Exception("A viagem solicitada para remarcar não consta no sistema!");
            
            var passagemValidaIda = Passagem.DataValida(dataOrigemIda, dataDestinoIda);
            var passagemValidaVolta = Passagem.DataValida(dataOrigemVolta, dataDestinoVolta);
            var viagemValida = Viagem.DataViagemValida(dataOrigemIda, dataOrigemVolta);
            if (!passagemValidaIda || !passagemValidaVolta)
                throw new Exception($"A data de origem não pode ser anterior à data de destino em nenhuma das passagens!");
            else if (!viagemValida)
                throw new Exception($"A data da passagem de volta não pode ser anterior à data da passagem de ida!");

            _viagemDAO.RemarcarViagemIda(viagemRemarcada.Id, dataOrigemIda, dataDestinoIda);
            _viagemDAO.RemarcarViagemVolta(viagemRemarcada.Id, dataOrigemVolta, dataDestinoVolta);

        }
        public List<Viagem> BuscarTodasViagens()
        {
            var listaViagens = _viagemDAO.BuscarTodasViagens();
            if (listaViagens.Count == 0)
                throw new Exception("Nenhuma viagem cadastrada!");
            return listaViagens;
        }
    }
}
