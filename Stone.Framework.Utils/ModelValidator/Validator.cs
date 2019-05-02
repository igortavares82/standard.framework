using System;
using System.Collections.Generic;
using System.Linq;
using CM = System.ComponentModel.DataAnnotations;

namespace Stone.Framework.Utils.ModelValidator
{
    public class Validator
    {
        public static Tuple<bool, List<string>> Validate(object model)
        {
            bool result = false;

            CM.ValidationContext context = new CM.ValidationContext(model);
            List<CM.ValidationResult> results = new List<CM.ValidationResult>();

            result = CM.Validator.TryValidateObject(model, context, results, true);

            return new Tuple<bool, List<string>>(result, results.Select(it => $"{it.MemberNames.First()}: {it.ErrorMessage}").ToList());
        }
    }
}
