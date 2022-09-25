using Microsoft.AspNetCore.Mvc;
using SerraLinhasAereasSolution.Domain.Entities;
using SerraLinhasAereasSolution.Domain.Interfaces;
using SerraLinhasAereasSolution.Infra.Data.DAO;
using System;
using System.Collections.Generic;


namespace SerraLinhasAereasSolution.Infra.Data.Repository
{
    public class PassagemRepository : IPassagemRepository
    {
        private readonly PassagemDAO _passagemDAO;
        public PassagemRepository()
        {
            _passagemDAO = new PassagemDAO();
        }
        public void AdicionarPassagem(Passagem passagem)
        {
            bool passagemValida = Passagem.DataValida(passagem.DataOrigem, passagem.DataDestino);
            if (passagemValida)
                _passagemDAO.InserirPassagem(passagem);
            else
                throw new Exception("A data de destino não pode ser anterior à data de origem!");
        }
        public List<Passagem> BuscarTodasPassagens()
        {
            var listaPassagens = _passagemDAO.BuscarTodasPassagens();
            if (listaPassagens.Count == 0)
                throw new Exception("Não há passagens registradas no sistema!");
            return listaPassagens;
        }
         public List<Passagem> BuscarPassagensPorData(DateTime data)
        {
            var listaPassagens = _passagemDAO.BuscarPassagensPorData(data);
            if (listaPassagens.Count == 0)
                throw new Exception($"Não há passagens registradas na data {data.ToShortDateString()}!");
            return listaPassagens;
        }
         public List<Passagem> BuscarPassagemPorOrigem(string origem)
        {
            var listaPassagens = _passagemDAO.BuscarPassagensPorOrigem(origem);
            if (listaPassagens.Count == 0)
                throw new Exception($"Não há passagens partindo de {origem} no sistema!");
            return listaPassagens;
        }
        public List<Passagem> BuscarPassagemPorDestino(string destino)
        {
            var listaPassagens = _passagemDAO.BuscarPassagensPorDestino(destino);
            if (listaPassagens.Count == 0)
                throw new Exception($"Não há passagens partindo para {destino} no sistema!");
            return listaPassagens;
        }
        public Passagem BuscarPassagemPorId(int id)
        {
            var passagemBuscada = _passagemDAO.BuscarPassagemPorId(id);
            if (passagemBuscada == null)
                throw new Exception($"Não há passagem com o id {id}!");
            return passagemBuscada;
        }
        public void AtualizarPassagem(Passagem passagemAtualizada)
        {
            if (passagemAtualizada == null)
                throw new Exception("Passagem não localizada.");
            else {
                bool passagemValida = Passagem.DataValida(passagemAtualizada.DataOrigem, passagemAtualizada.DataDestino);
                if (passagemValida)
                    _passagemDAO.AtualizaPassagem(passagemAtualizada);
                else
                    throw new Exception("A data de destino não pode ser anterior à data de origem!");
                }
        }
    }
}
