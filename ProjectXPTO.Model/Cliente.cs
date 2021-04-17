using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectXPTO.Model
{
    public class Cliente : BaseClass
    {
        public string Nome { get; set; }

        public string Email { get; set; }

        public DateTime DataNascimento { get; set; }

        public string Cpf { get; set; }


    }
}
