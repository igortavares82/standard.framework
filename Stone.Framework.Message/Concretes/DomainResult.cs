using Stone.Framework.Result.Abstractions;
using Stone.Framework.Result.Enums;
using System.Collections.Generic;

namespace Stone.Framework.Result.Concretes
{
    public class DomainResult<T> : IDomainResult<T>
    {
        public T Data { get; set; }
        public DomainResultType ResultType { get; set; }
        public List<string> Messages { get; set; } = new List<string>();
    }
}
