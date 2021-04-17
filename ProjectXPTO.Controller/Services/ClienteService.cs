using System;
using System.Collections.Generic;
using ProjectXPTO.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectXPTO.Controller.Services
{
    public class ClienteService : BaseService<Cliente>
    {
        public  void Cliente (string nome, string email, DateTime datanascimento, string cpf)
        {
            cpf = cpf.Replace(".", "").Replace(".", "");

            ValidarNome(nome);
            ValidarEmail(email);
            ValidarCpf(cpf);

            cpf = "#########" + cpf.Substring(7, 2);
        }
        private void ValidarNome(string nome)
        {
            if(nome.Length < 3)
            {
                throw new Exception("Nome possui menos que 3 caracteres.");
            }

            if (nome.Length > 50)
            {
                throw new Exception("Nome possui mais que 50 caracteristicas");
            }
            if(nome.Contains("!") || nome.Contains("@"))
            {
                throw new Exception("Nome contém caracacters especeiasi");
            }
        }
        private void ValidarEmail(string email)
        {
            if(email.Length < 3 || email.Length > 100)
            {
                throw new Exception("Quantidade de caracteres do email é invalido. é necessario ser maior do que 3 ou menis que 100");
            }
        }
        private void ValidarCpf(string cpf)
        {
            if(cpf.Length != 11)
            {
                throw new Exception("O cpf inforemado é invalido.");
            }
        }
    }
}
