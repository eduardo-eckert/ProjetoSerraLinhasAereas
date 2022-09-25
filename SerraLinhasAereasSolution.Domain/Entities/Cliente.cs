using System;

namespace SerraLinhasAereasSolution.Domain.Entities
{
    public class Cliente
    {
       public int Id { get; set; }
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string NomeCompleto { get; set; }
        public string Cep { get; set; }
        public string Rua { get; set; }
        public string Bairro { get; set; }
        public int Numero { get; set; }
        public string Complemento { get; set; }
        public Cliente()
        {

        }
        public Cliente(int id, string cpf, string nome, string sobrenome, string cep, string rua, string bairro, int numero, string complemento)
        {
            Id = id;
            Cpf = cpf;
            Nome = nome;
            Sobrenome = sobrenome;
            NomeCompleto = $"{nome} {sobrenome}";
            Cep = cep;
            Rua = rua;
            Bairro = bairro;
            Numero = numero;
            Complemento = complemento;
        }
        public Cliente(string cpf)
        {
            this.Cpf = cpf;
        }
        public static bool CpfValido(string cpf)
        {
            return cpf.Length == 11;
        }
    }
}
