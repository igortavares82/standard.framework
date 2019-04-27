using System.ComponentModel.DataAnnotations;

namespace Stone.Framework.Validator.Concretes
{
    public class CpfAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            bool result = false;
            string cpf = value as string;

            int[] mt1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] mt2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCPF = string.Empty;
            string digit = string.Empty;
            int sum = 0;
            int mod = 0;

            cpf = cpf.Trim();
            cpf = cpf.Replace(".", string.Empty).Replace("-", string.Empty);

            if (cpf.Length != 11)
                return false;

            tempCPF = cpf.Substring(0, 9);

            for (int i = 0; i < 9; i++)
                sum += int.Parse(tempCPF[i].ToString()) * mt1[i];

            mod = sum % 11;

            if (mod < 2)
                mod = 0;
            else
                mod = 11 - mod;

            digit = mod.ToString();
            tempCPF = tempCPF + digit;
            sum = 0;

            for (int i = 0; i < 10; i++)
                sum += int.Parse(tempCPF[i].ToString()) * mt2[i];

            mod = sum % 11;

            if (mod < 2)
                mod = 0;
            else
                mod = 11 - mod;

            digit = digit + mod.ToString();
            result = cpf.EndsWith(digit);

            return result;
        }
    }
}
