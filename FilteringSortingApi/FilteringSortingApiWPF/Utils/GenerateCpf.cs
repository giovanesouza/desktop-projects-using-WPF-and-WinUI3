using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FilteringSortingApiWPF.Utils
{
    static class GenerateCpf
    {

        public static string Create()
        {
            Random random = new Random();
            int[] cpf = new int[11];

            // Gerar os primeiros 9 dígitos aleatoriamente
            for (int i = 0; i < 9; i++)
            {
                cpf[i] = random.Next(0, 10);
            }

            // Calcular o primeiro dígito verificador
            cpf[9] = CalculateCheckDigit(cpf, 9);

            // Calcular o segundo dígito verificador
            cpf[10] = CalculateCheckDigit(cpf, 10);

            // Formatar o CPF
            return string.Format("{0:000\\.000\\.000\\-00}", long.Parse(string.Join("", cpf)));
        }

        static int CalculateCheckDigit(int[] cpf, int tamanho)
        {
            int soma = 0;
            for (int i = 0; i < tamanho; i++)
            {
                soma += cpf[i] * (tamanho + 1 - i);
            }

            int resto = soma % 11;
            return resto < 2 ? 0 : 11 - resto;
        }


    }
}
