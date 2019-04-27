using Stone.Framework.Result.Abstractions;
using Stone.Framework.Result.Concretes;
using Stone.Framework.Result.Enums;
using System;
using System.Net;

namespace Stone.Framework.Result.Mappers
{
    public class ResultMapper
    {
        public static IApplicationResult<TApplication> MapFromDomainResult<TDomain, TApplication>(IDomainResult<TDomain> domainResult, Func<TDomain,TApplication> mapper)
        {
            IApplicationResult<TApplication> applicationResult = new ApplicationResult<TApplication>()
            {
                Data = mapper(domainResult.Data),
                Messages = domainResult.Messages,
                StatusCode = GetStatusCode(domainResult.ResulType)
            };

            return applicationResult;
        }

        private static HttpStatusCode GetStatusCode(DomainResultType resultType)
        {
            HttpStatusCode statusCode = HttpStatusCode.Unused;

            switch (resultType)
            {
                case DomainResultType.Success:
                    statusCode = HttpStatusCode.OK;
                    break;

                case DomainResultType.DomainError:
                    statusCode = HttpStatusCode.BadRequest;
                    break;

                case DomainResultType.SystemError:
                    statusCode = HttpStatusCode.InternalServerError;
                    break;
            }

            return statusCode;
        }
    }
}
