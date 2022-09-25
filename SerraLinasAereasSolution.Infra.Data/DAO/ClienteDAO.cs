using SerraLinhasAereasSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SerraLinhasAereasSolution.Infra.Data.DAO
{
    public class ClienteDAO
    {
        private readonly string _connectionString =
            @"server=.\SQLexpress;initial catalog=SerraLinhasAereasDB;integrated security=true;";

        public void InserirCliente(Cliente novoCliente)
        {
            using (var conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    string sql =
                        @"INSERT CLIENTES(CPF, NOME, SOBRENOME, CEP, RUA, BAIRRO, NUMERO, COMPLEMENTO) VALUES (@CPF, @NOME, @SOBRENOME, @CEP, @RUA, @BAIRRO, @NUMERO, @COMPLEMENTO)";

                    ConverterObjetoParaSQL(novoCliente, comando);
                    comando.CommandText = sql;
                    comando.ExecuteNonQuery();
                }
            }
        }
        public void AtualizarCliente(Cliente clienteEditado)
        {
            using (var conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    string sql =
                        @"UPDATE CLIENTES SET CPF = @CPF, NOME = @NOME, SOBRENOME = @SOBRENOME, CEP = @CEP, RUA = @RUA,
                                BAIRRO = @BAIRRO, NUMERO = @NUMERO, COMPLEMENTO = @COMPLEMENTO
                                WHERE ID = @ID";

                    ConverterObjetoParaSQL(clienteEditado, comando);
                    comando.CommandText = sql;
                    comando.ExecuteNonQuery();
                }
            }
        }
        public List<Cliente> BuscarTodosClientes()
        {
            var listaClientes = new List<Cliente>();
            using (var conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    string sql =
                        @"SELECT ID, CPF, NOME, SOBRENOME, CEP, RUA, BAIRRO, NUMERO, COMPLEMENTO FROM CLIENTES;";
                    comando.CommandText = sql;
                    var leitor = comando.ExecuteReader();

                    while (leitor.Read())
                    {
                        var clienteBuscado = ConverterSqlParaObjeto(leitor);
                         listaClientes.Add(clienteBuscado);
                    }
                }
            }
            return listaClientes;
        }
        public Cliente BuscarClientePorCpf(string cpfDigitado)
        {
            using (var conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    string sql =
                        @"SELECT ID, CPF, NOME, SOBRENOME, CEP, RUA, BAIRRO, NUMERO, COMPLEMENTO
                                   FROM CLIENTES WHERE CPF = @CPF;";
                    comando.Parameters.AddWithValue("@CPF", cpfDigitado);
                    comando.CommandText = sql;
                    var leitor = comando.ExecuteReader();
                    while (leitor.Read())
                    {
                        var clienteBuscado = ConverterSqlParaObjeto(leitor);
                        return clienteBuscado;
                    }
                }
            }
            return null;
        }
        public void DeletarCliente(string cpfDigitado)
        {
            using (var conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    string sql = @"DELETE FROM CLIENTES WHERE CPF = @CPF;";
                    comando.Parameters.AddWithValue("@CPF", cpfDigitado);
                    comando.CommandText = sql;
                    comando.ExecuteNonQuery();
                }
            }
        }
        private void ConverterObjetoParaSQL(Cliente novoCliente, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("@ID", novoCliente.Id);
            comando.Parameters.AddWithValue("@CPF", novoCliente.Cpf);
            comando.Parameters.AddWithValue("@NOME", novoCliente.Nome);
            comando.Parameters.AddWithValue("@SOBRENOME", novoCliente.Sobrenome);
            comando.Parameters.AddWithValue("@CEP", novoCliente.Cep);
            comando.Parameters.AddWithValue("@RUA", novoCliente.Rua);
            comando.Parameters.AddWithValue("@BAIRRO", novoCliente.Bairro);
            comando.Parameters.AddWithValue("@NUMERO", novoCliente.Numero);
            comando.Parameters.AddWithValue("@COMPLEMENTO", novoCliente.Complemento);
        }
        private Cliente ConverterSqlParaObjeto(SqlDataReader leitor)
        {
            var id = Convert.ToInt32(leitor["ID"].ToString());
            var cpf = leitor["CPF"].ToString();
            var nome = leitor["NOME"].ToString();
            var sobrenome = leitor["SOBRENOME"].ToString();
            var cep = leitor["CEP"].ToString();
            var rua = leitor["RUA"].ToString();
            var bairro = leitor["BAIRRO"].ToString();
            var numero = Convert.ToInt32(leitor["NUMERO"].ToString());
            var complemento = leitor["COMPLEMENTO"].ToString();
            return new Cliente(id, cpf, nome, sobrenome, cep, rua, bairro, numero, complemento);
        }
    }
}
