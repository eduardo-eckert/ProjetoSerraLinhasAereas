using SerraLinhasAereasSolution.Domain.Entities;
using SerraLinhasAereasSolution.Domain.Interfaces;
using SerraLinhasAereasSolution.Infra.Data.DAO;
using System;
using System.Collections.Generic;


namespace SerraLinhasAereasSolution.Infra.Data.Repository
{   
    public class ClienteRepository : IClienteRepository
    {
        private readonly ClienteDAO _clienteDAO;
        
        public ClienteRepository()
        {
            _clienteDAO = new ClienteDAO();
        }
        public void RegistrarCliente(Cliente cliente)
        {
            bool cpfValido = Cliente.CpfValido(cliente.Cpf);
            if (cpfValido)
            {
                var clienteBuscado = _clienteDAO.BuscarClientePorCpf(cliente.Cpf);
                if (clienteBuscado == null)
                    _clienteDAO.InserirCliente(cliente);
                else
                    throw new Exception($"O cliente com CPF {cliente.Cpf} já está cadastrado no sistema!");
            }
            else
                throw new Exception($"O CPF {cliente.Cpf} é inválido.");
        }
        public Cliente BuscarClientePorCpf(string cpf)
        {
            bool cpfValido = Cliente.CpfValido(cpf);
            if (cpfValido)
            {
                var clienteBuscado = _clienteDAO.BuscarClientePorCpf(cpf);
                if (clienteBuscado == null)
                    throw new Exception($"O cliente com o CPF {cpf} não foi encontrado!");
                else
                    return clienteBuscado;
            }
            else
                throw new Exception($"O CPF {cpf} não é válido!");
        }
        public List<Cliente> BuscarTodosClientes()
        {
            var listaClientes = _clienteDAO.BuscarTodosClientes();
            if (listaClientes.Count == 0)
                throw new Exception("Nenhum cliente cadastrado!");
            return listaClientes;
        }
        public void AtualizarCliente(Cliente clienteAtualizado)
        {
            bool cpfValido = Cliente.CpfValido(clienteAtualizado.Cpf);
            if (cpfValido)
            {
                var buscaCliente = _clienteDAO.BuscarClientePorCpf(clienteAtualizado.Cpf);
                
                if (buscaCliente == null)
                    throw new Exception($"Cliente não encontrado!");
                else
                    _clienteDAO.AtualizarCliente(clienteAtualizado);
            }
            else
                throw new Exception($"O CPF {clienteAtualizado.Cpf} não é válido!");
        }
        public void DeletarCliente(string cpf)
        {
            bool cpfValido = Cliente.CpfValido(cpf);
            if (cpfValido)
            {
                var clienteBuscado = _clienteDAO.BuscarClientePorCpf(cpf);
                if (clienteBuscado == null)
                    throw new Exception($"O cliente com o CPF {cpf} não foi encontrado!");
                else
                    _clienteDAO.DeletarCliente(clienteBuscado.Cpf);
            }
            else
                throw new Exception($"O CPF {cpf} não é válido.");
        }
    }
}
