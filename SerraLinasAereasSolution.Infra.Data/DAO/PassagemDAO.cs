using SerraLinhasAereasSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SerraLinhasAereasSolution.Infra.Data.DAO
{
    public class PassagemDAO
    {
        private readonly string _connectionString = @"server=.\SQLexpress;initial catalog=SerraLinhasAereasDB;integrated security=true;";
        public void InserirPassagem(Passagem novaPassagem)
        {
            using (var conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    string sql = @"INSERT INTO PASSAGENS VALUES (@ORIGEM, @DESTINO, @VALOR, @RESERVADA, @DATAORIGEM, @DATADESTINO);";
                    ConverterObjetoParaSQL(novaPassagem, comando);
                    comando.CommandText = sql;
                    comando.ExecuteNonQuery();
                }
            }
        }
        public List<Passagem> BuscarTodasPassagens()
        { 
            var listaPassagens = new List<Passagem>();
            using (var conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    string sql = @"SELECT ID, ORIGEM, DESTINO, VALOR, RESERVADA, DATAORIGEM, DATADESTINO FROM PASSAGENS";
                    comando.CommandText = sql;
                    var leitor = comando.ExecuteReader();
                    while (leitor.Read())
                    {
                        var passagem = ConverterSqlParaObjeto(leitor);
                        listaPassagens.Add(passagem);
                    }
                }
            }
            return listaPassagens;
        }
        public List<Passagem> BuscarPassagensPorData(DateTime dataBuscada)
        {
            var listaPassagens = new List<Passagem>();
            using (var conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    string sql = @"SELECT ID, ORIGEM, DESTINO, VALOR, RESERVADA, DATAORIGEM, DATADESTINO FROM PASSAGENS
                                   WHERE (CONVERT(DATE, DATAORIGEM) = CONVERT(DATE, @DATABUSCADA) OR
                                   CONVERT(DATE, DATADESTINO) = CONVERT(DATE, @DATABUSCADA));";
                    comando.Parameters.AddWithValue("@DATABUSCADA", dataBuscada);
                    comando.CommandText = sql;
                    var leitor = comando.ExecuteReader();
                    while (leitor.Read())
                    {
                        var passagem = ConverterSqlParaObjeto(leitor);
                        listaPassagens.Add(passagem);
                    }
                }
            }
            return listaPassagens;
        }
        public List<Passagem> BuscarPassagensPorOrigem(string origemBuscada)
        {
            var listaPassagens = new List<Passagem>();
            using (var conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    string sql = @"SELECT ID, ORIGEM, DESTINO, VALOR, RESERVADA, DATAORIGEM, DATADESTINO FROM PASSAGENS
                                    WHERE ORIGEM = @ORIGEMBUSCADA;";
                    comando.Parameters.AddWithValue("@ORIGEMBUSCADA", origemBuscada);
                    comando.CommandText = sql;
                    var leitor = comando.ExecuteReader();
                    while (leitor.Read())
                    {
                        var passagem = ConverterSqlParaObjeto(leitor);
                        listaPassagens.Add(passagem);
                    }
                }
            }
            return listaPassagens;
        }
        public List<Passagem> BuscarPassagensPorDestino(string destinoBuscado)
        {
            var listaPassagens = new List<Passagem>();
            using (var conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    string sql = @"SELECT ID, ORIGEM, DESTINO, VALOR, RESERVADA, DATAORIGEM, DATADESTINO FROM PASSAGENS
                                    WHERE DESTINO = @DESTINOBUSCADO;";
                    comando.Parameters.AddWithValue("@DESTINOBUSCADO", destinoBuscado);
                    comando.CommandText = sql;
                    var leitor = comando.ExecuteReader();
                    while (leitor.Read())
                    {
                        var passagem = ConverterSqlParaObjeto(leitor);
                        listaPassagens.Add(passagem);
                    }
                }
            }
            return listaPassagens;
        }
        public Passagem BuscarPassagemPorId(int id)
        {
            var listaPassagens = new List<Passagem>();
            using (var conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    string sql = @"SELECT ID, ORIGEM, DESTINO, VALOR, RESERVADA, DATAORIGEM, DATADESTINO FROM PASSAGENS
                                   WHERE ID = @ID";
                    comando.Parameters.AddWithValue("@ID", id);
                    comando.CommandText = sql;
                    var leitor = comando.ExecuteReader();
                    while (leitor.Read())
                    {
                        var passagem = ConverterSqlParaObjeto(leitor);
                        return passagem;
                    }
                }
            }
            return null;
        }
        public void AtualizaPassagem(Passagem passagemEditada)
        {
            using (var conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    string sql = @"UPDATE PASSAGENS SET ORIGEM = @ORIGEM, DESTINO = @DESTINO, VALOR = @VALOR, RESERVADA = @RESERVADA,
                                   DATAORIGEM = @DATAORIGEM, DATADESTINO = @DATADESTINO WHERE ID = @ID;";

                    ConverterObjetoParaSQL(passagemEditada, comando);
                    comando.CommandText = sql;
                    comando.ExecuteNonQuery();
                }
            }
        }
        public void ReservarPassagem(Passagem passagemReservada)
        {
            using (var conexao = new SqlConnection(_connectionString))
            {
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    string sql = @"UPDATE PASSAGENS SET RESERVADA = @RESERVADA WHERE ID = @ID;";
                    comando.Parameters.AddWithValue("@ID", passagemReservada.Id);
                    comando.Parameters.AddWithValue("@RESERVADA", passagemReservada.Reservada);
                    comando.CommandText = sql;
                    comando.ExecuteNonQuery();
                }
            }
        }
        private void ConverterObjetoParaSQL(Passagem novaPassagem, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("@ID", novaPassagem.Id);
            comando.Parameters.AddWithValue("@ORIGEM", novaPassagem.Origem);
            comando.Parameters.AddWithValue("@DESTINO", novaPassagem.Destino);
            comando.Parameters.AddWithValue("@VALOR", novaPassagem.Valor);
            comando.Parameters.AddWithValue("@RESERVADA", novaPassagem.Reservada);
            comando.Parameters.AddWithValue("@DATAORIGEM", novaPassagem.DataOrigem);
            comando.Parameters.AddWithValue("@DATADESTINO", novaPassagem.DataDestino);
        }
        private Passagem ConverterSqlParaObjeto(SqlDataReader leitor)
        {
            var idPassagem = Convert.ToInt32(leitor["ID"].ToString());
            var origem = leitor["ORIGEM"].ToString();
            var destino = leitor["DESTINO"].ToString();
            var valor = Convert.ToDecimal(leitor["VALOR"].ToString());
            var reservada = Convert.ToBoolean(leitor["RESERVADA"].ToString());
            var dataOrigem = Convert.ToDateTime(leitor["DATAORIGEM"].ToString());
            var dataDestino = Convert.ToDateTime(leitor["DATADESTINO"].ToString());
            return new Passagem(idPassagem, origem, destino, valor, reservada, dataOrigem, dataDestino);
        }
    }
}
