using Microsoft.AspNetCore.Mvc;
using SerraLinhasAereasSolution.Domain.Entities;
using SerraLinhasAereasSolution.Domain.Interfaces;
using SerraLinhasAereasSolution.Infra.Data.Repository;
using System;
using System.Collections.Generic;

namespace SerraLinhasAereasSolution.WebAPI.Controllers
{
    [ApiController]
    [Route("api/viagens")]
    public class ViagemController : ControllerBase
    {
        private readonly IViagemRepository _viagemRepository;
        public ViagemController()
        {
            _viagemRepository = new ViagemRepository();
        }
        [HttpPost]
        public IActionResult PostViagem(Viagem novaViagem)
        {
            try
            {
                _viagemRepository.MarcarViagem(novaViagem);
                return Ok(new Resposta(200, $"Viagem marcada! {novaViagem.ResumoDaViagem}"));
            }
            catch (Exception e)
            {
                return StatusCode(500, new Resposta(500, e.Message));
            }
        }
        [HttpGet]
        public IActionResult GetTodasViagens()
        {
            try
            {
                return Ok(_viagemRepository.BuscarTodasViagens());
            }
            catch (Exception e)
            {
                return StatusCode(500, new Resposta(500, e.Message));
            }
        }
        [HttpGet("buscar-por-cliente")]
        public IActionResult GetViagensPorCliente([FromQuery] string cpf)
        {
            try
            {
                return Ok(_viagemRepository.BuscarViagensPorCliente(cpf));
            }
            catch (Exception e)
            {
                return StatusCode(500, new Resposta(500, e.Message));
            }
        }
        [HttpPatch("ida")]
        public IActionResult PatchViagemIda([FromQuery] int idViagem, DateTime dataOrigem, DateTime dataDestino)
        {
            try
            {
                _viagemRepository.RemarcarViagemIda(idViagem, dataOrigem, dataDestino);
                return Ok(new Resposta(200, $"Viagem {idViagem} remarcada, passagem alterada com sucesso. Sua nova data de partida é em {dataOrigem} e a chegada está prevista para {dataDestino}."));
            }
            catch (Exception e)
            {
                return StatusCode(500, new Resposta(500, e.Message));
            }
        }

        [HttpPatch("idaEVolta")]
        public IActionResult PatchViagemIdaEVolta([FromQuery] int idViagem, DateTime dataOrigemIda, DateTime dataDestinoIda, DateTime dataOrigemVolta, DateTime dataDestinoVolta)
        {
            try
            {
                _viagemRepository.RemarcarViagemIdaEVolta(idViagem, dataOrigemIda, dataDestinoIda, dataOrigemVolta, dataDestinoVolta);
                return Ok(new Resposta(200, $"Viagem {idViagem} remarcada, passagens alteradas com sucesso! Sua nova data de partida é em {dataOrigemIda} e a chegada está prevista para {dataOrigemVolta}."));
            }
            catch (Exception e)
            {
                return StatusCode(500, new Resposta(500, e.Message));
            }
        }
    }
}
