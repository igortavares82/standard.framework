using Stone.Framework.Result.Enums;
using System.Collections.Generic;

namespace Stone.Framework.Result.Abstractions
{
    public interface IDomainResult<T>
    {
        T Data { get; set; }
        DomainResultType ResulType { get; }
        List<string> Messages { get; set; }
    }
}
