using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.Windows.Controls;

namespace RegisterWPF.Exceptions
{
    public static class ClientValidations
    {

        private static readonly Regex regexName = new Regex(@"^[aA-zZ|\s]{3,}$");
        private static readonly Regex regexCpf = new Regex(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$");

        public static void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("O campo 'Nome' é obrigatório!");

            if (!regexName.IsMatch(name))
                throw new ValidationException("O campo 'Nome' deve conter apenas letras (mín. 3).");
        }

        public static void ValidateCpf(string cpf)
        {
            cpf = cpf.Replace(".", "").Replace("-", "");

            if (string.IsNullOrWhiteSpace(cpf))
                throw new ValidationException("O campo 'Cpf' é obrigatório!");

            if (!Regex.IsMatch(cpf, @"^\d{11}$"))
                throw new ValidationException("O CPF deve conter 11 dígitos.");

            if(cpf.Length == 11)
                if (!IsValidCpf(cpf)) throw new ValidationException("CPF inválido.");
        }

        public static string FormatCpf(string cpf)
        {
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length == 11)
                return Regex.Replace(cpf, @"(\d{3})(\d{3})(\d{3})(\d{2})", "$1.$2.$3-$4");

            return cpf;
        }

        public static bool IsValidCpf(string cpf)
        {
            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;

            cpf = cpf.Trim().Replace(".", "").Replace("-", "");

            if (cpf.Length != 11)
                return false;

            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            resto = soma % 11;
            digito = (resto < 2 ? 0 : 11 - resto).ToString();
            tempCpf += digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            digito += (resto < 2 ? 0 : 11 - resto).ToString();

            return cpf.EndsWith(digito);
        }



    }
}
