using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterWPF.Models
{
    class Client
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Cpf { get; private set; }
        public DateTime DateRegister { get; private set; }

        public Client(int id, string nome, string cpf)
        {
            Id = id;
            Name = nome;
            Cpf = cpf;
            DateRegister = DateTime.Now;
        }

    }
}
