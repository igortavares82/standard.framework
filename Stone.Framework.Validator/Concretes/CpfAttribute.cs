using Stone.Framework.Utils.Cpf;
using System.ComponentModel.DataAnnotations;

namespace Stone.Framework.Validator.Concretes
{
    public class CpfAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return CpfValidator.Validate(value as string);
        }
    }
}
