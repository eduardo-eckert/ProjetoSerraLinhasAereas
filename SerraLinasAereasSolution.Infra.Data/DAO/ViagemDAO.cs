using SerraLinhasAereasSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerraLinhasAereasSolution.Infra.Data.DAO
{
    public class ViagemDAO
    {
        private ClienteDAO _clienteDAO = new ClienteDAO();
        private PassagemDAO _passagemDAO = new PassagemDAO();
        private readonly string _connectionString = @"server=.\SQLexpress;initial catalog=SerraLinhasAereasDB;integrated security=true;";
        public void MarcarViagem(Viagem novaViagem)
        {
            using (var conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    string sql = @"INSERT VIAGENS VALUES (@CODIGORESERVA, @DATACOMPRA, @VALORTOTAL, @CLIENTE_ID, @SOMENTEIDA, @PASSAGEMIDA_ID, @PASSAGEMVOLTA_ID);";
                    comando.Parameters.AddWithValue("@CODIGORESERVA", novaViagem.CodigoReserva);
                    comando.Parameters.AddWithValue("@DATACOMPRA", novaViagem.DataCompra);
                    comando.Parameters.AddWithValue("@VALORTOTAL", novaViagem.ValorTotal);
                    comando.Parameters.AddWithValue("@CLIENTE_ID", novaViagem.Cliente.Id);
                    comando.Parameters.AddWithValue("@SOMENTEIDA", novaViagem.SomenteIda);
                    comando.Parameters.AddWithValue("@PASSAGEMIDA_ID", novaViagem.PassagemIda.Id);
                    comando.Parameters.AddWithValue("@PASSAGEMVOLTA_ID", novaViagem.PassagemVolta != null ? novaViagem.PassagemVolta.Id : DBNull.Value);
                    comando.CommandText = sql;
                    comando.ExecuteNonQuery();
                }
            }
        }
        public List<Viagem> BuscaViagensPorCliente(Cliente clienteBuscado)
        {
            var listaViagens = new List<Viagem>();
            using (var conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    string sql = @"SELECT V.*, C.*, PI.ID AS IDAID, PI.ORIGEM AS IDAORIGEM, PI.DESTINO AS IDADESTINO, PI.VALOR AS IDAVALOR, PI.DATAORIGEM AS IDADATAORIGEM, PI.DATADESTINO AS IDADATADESTINO,  
                                   PV.ID AS VOLTAID, PV.ORIGEM AS VOLTAORIGEM, PV.DESTINO AS VOLTADESTINO, PV.VALOR AS VOLTAVALOR, PV.DATAORIGEM AS VOLTADATAORIGEM, PV.DATADESTINO AS VOLTADATADESTINO                                   
                                   FROM VIAGENS V JOIN CLIENTES C ON V.CLIENTE_ID = C.ID
                                   INNER JOIN PASSAGENS PI ON PI.ID = V.PASSAGEMIDA_ID 
                                   LEFT JOIN PASSAGENS PV ON PV.ID = V.PASSAGEMVOLTA_ID
                                   WHERE V.CLIENTE_ID = @IDBUSCADO";
                    comando.Parameters.AddWithValue("@IDBUSCADO", clienteBuscado.Id);
                    comando.CommandText = sql;
                    var leitor = comando.ExecuteReader();
                    while (leitor.Read())
                    {
                        var viagem = ConverterSqlParaObjeto(leitor);
                        listaViagens.Add(viagem);
                    }
                }
            }
            return listaViagens;
        }
        public Viagem BuscaViagensPorId(int idViagem)
        {
            using (var conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    string sql = @"SELECT V.*, C.*, PI.ID AS IDAID, PI.ORIGEM AS IDAORIGEM, PI.DESTINO AS IDADESTINO, PI.VALOR AS IDAVALOR, PI.DATAORIGEM AS IDADATAORIGEM, PI.DATADESTINO AS IDADATADESTINO,  
                                   PV.ID AS VOLTAID, PV.ORIGEM AS VOLTAORIGEM, PV.DESTINO AS VOLTADESTINO, PV.VALOR AS VOLTAVALOR, PV.DATAORIGEM AS VOLTADATAORIGEM, PV.DATADESTINO AS VOLTADATADESTINO                                   
                                   FROM VIAGENS V JOIN CLIENTES C ON V.CLIENTE_ID = C.ID
                                   INNER JOIN PASSAGENS PI ON PI.ID = V.PASSAGEMIDA_ID 
                                   LEFT JOIN PASSAGENS PV ON PV.ID = V.PASSAGEMVOLTA_ID
                                   WHERE V.ID = @IDBUSCADO";
                    comando.Parameters.AddWithValue("@IDBUSCADO", idViagem);
                    comando.CommandText = sql;
                    var leitor = comando.ExecuteReader();
                    while (leitor.Read())
                    {
                        var viagem = ConverterSqlParaObjeto(leitor);
                        return viagem;
                    }
                }
            }
            return null;
        }
        public List<Viagem> BuscarTodasViagens()
        {
            var listaViagens = new List<Viagem>();
            using (var conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    string sql = @"SELECT V.*, C.*, PI.ID AS IDAID, PI.ORIGEM AS IDAORIGEM, PI.DESTINO AS IDADESTINO, PI.VALOR AS IDAVALOR, PI.DATAORIGEM AS IDADATAORIGEM, PI.DATADESTINO AS IDADATADESTINO,  
                                   PV.ID AS VOLTAID, PV.ORIGEM AS VOLTAORIGEM, PV.DESTINO AS VOLTADESTINO, PV.VALOR AS VOLTAVALOR, PV.DATAORIGEM AS VOLTADATAORIGEM, PV.DATADESTINO AS VOLTADATADESTINO                                   
                                   FROM VIAGENS V JOIN CLIENTES C ON V.CLIENTE_ID = C.ID
                                   INNER JOIN PASSAGENS PI ON PI.ID = V.PASSAGEMIDA_ID 
                                   LEFT JOIN PASSAGENS PV ON PV.ID = V.PASSAGEMVOLTA_ID";
                    comando.CommandText = sql;
                    var leitor = comando.ExecuteReader();
                    while (leitor.Read())
                    {
                        var viagem = ConverterSqlParaObjeto(leitor);
                        listaViagens.Add(viagem);
                    }
                }
            }
            return listaViagens;
        }
        public void RemarcarViagemIda(int idViagem, DateTime dataOrigem, DateTime dataDestino)
        {
            using (var conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    string sql = @"UPDATE PASSAGENS SET DATAORIGEM = @DATAORIGEM, DATADESTINO = @DATADESTINO
                                   FROM PASSAGENS P INNER JOIN VIAGENS V ON P.ID = V.PASSAGEMIDA_ID
                                   WHERE V.ID = @IDVIAGEM";
                    comando.Parameters.AddWithValue("@DATAORIGEM", dataOrigem);
                    comando.Parameters.AddWithValue("@DATADESTINO", dataDestino);
                    comando.Parameters.AddWithValue("@IDVIAGEM", idViagem);
                    comando.CommandText = sql;
                    comando.ExecuteNonQuery();
                }
            }
        }
        public void RemarcarViagemVolta(int idViagem, DateTime dataOrigem, DateTime dataDestino)
        {
            using (var conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    string sql = @"UPDATE PASSAGENS SET DATAORIGEM = @DATAORIGEM, DATADESTINO = @DATADESTINO
                                   FROM PASSAGENS P INNER JOIN VIAGENS V ON P.ID = V.PASSAGEMVOLTA_ID
                                   WHERE V.ID = @IDVIAGEM";
                    comando.Parameters.AddWithValue("@DATAORIGEM", dataOrigem);
                    comando.Parameters.AddWithValue("@DATADESTINO", dataDestino);
                    comando.Parameters.AddWithValue("@IDVIAGEM", idViagem);
                    comando.CommandText = sql;
                    comando.ExecuteNonQuery();
                }
            }
        }
        private Viagem ConverterSqlParaObjeto(SqlDataReader leitor)
        {
            var id = Convert.ToInt32(leitor["ID"].ToString());
            var codigo = leitor["CODIGORESERVA"].ToString();
            var somenteIda = Convert.ToBoolean(leitor["SOMENTEIDA"].ToString());
            var cliente = new Cliente();
            cliente.Cpf = leitor["CPF"].ToString();
            cliente.Nome = leitor["NOME"].ToString();
            cliente.Sobrenome = leitor["SOBRENOME"].ToString();
            cliente.Cep = leitor["CEP"].ToString();
            cliente.Rua = leitor["RUA"].ToString();
            cliente.Bairro = leitor["BAIRRO"].ToString();
            cliente.Numero = Convert.ToInt32(leitor["NUMERO"].ToString());
            cliente.Complemento = leitor["COMPLEMENTO"].ToString();
            var passagemIda = new Passagem();
            passagemIda.Id = Convert.ToInt32(leitor["IDAID"].ToString());
            passagemIda.Origem = leitor["IDAORIGEM"].ToString();
            passagemIda.Destino = leitor["IDADESTINO"].ToString();
            passagemIda.Valor = Convert.ToDecimal(leitor["IDAVALOR"].ToString());
            passagemIda.DataOrigem = Convert.ToDateTime(leitor["IDADATAORIGEM"].ToString());
            passagemIda.DataDestino = Convert.ToDateTime(leitor["IDADATADESTINO"].ToString());
            var passagemVolta = new Passagem();
            if (!somenteIda)
            {
                passagemVolta.Id = Convert.ToInt32(leitor["VOLTAID"].ToString());
                passagemVolta.Origem = leitor["VOLTAORIGEM"].ToString();
                passagemVolta.Destino = leitor["VOLTADESTINO"].ToString();
                passagemVolta.Valor = Convert.ToDecimal(leitor["VOLTAVALOR"].ToString());
                passagemVolta.DataOrigem = Convert.ToDateTime(leitor["VOLTADATAORIGEM"].ToString());
                passagemVolta.DataDestino = Convert.ToDateTime(leitor["VOLTADATADESTINO"].ToString());
            }
            return new Viagem(id, codigo, cliente, somenteIda, passagemIda, passagemVolta);
        }
    }
}
