using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MvcSaed.Attributes
{
    public class CpfValidationAttribute : ValidationAttribute
    {
        public CpfValidationAttribute()
        {
            ErrorMessage = "CPF inválido";
        }

        public override bool IsValid(object? value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return true; // Deixa a validação Required lidar com campos obrigatórios

            string cpf = value.ToString()!;
            return IsValidCpf(cpf);
        }

        private static bool IsValidCpf(string cpf)
        {
            // Remove caracteres não numéricos
            cpf = Regex.Replace(cpf, @"[^\d]", "");

            // Verifica se tem 11 dígitos
            if (cpf.Length != 11)
                return false;

            // Verifica se todos os dígitos são iguais
            if (cpf.All(c => c == cpf[0]))
                return false;

            // Calcula o primeiro dígito verificador
            int sum = 0;
            for (int i = 0; i < 9; i++)
                sum += int.Parse(cpf[i].ToString()) * (10 - i);

            int remainder = sum % 11;
            int firstDigit = remainder < 2 ? 0 : 11 - remainder;

            if (int.Parse(cpf[9].ToString()) != firstDigit)
                return false;

            // Calcula o segundo dígito verificador
            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += int.Parse(cpf[i].ToString()) * (11 - i);

            remainder = sum % 11;
            int secondDigit = remainder < 2 ? 0 : 11 - remainder;

            return int.Parse(cpf[10].ToString()) == secondDigit;
        }
    }
}
