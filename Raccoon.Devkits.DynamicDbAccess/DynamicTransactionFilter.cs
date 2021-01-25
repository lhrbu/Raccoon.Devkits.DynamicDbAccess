using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Raccoon.Devkits.DynamicDbAccess
{
    public class DynamicTransactionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            DynamicTransactionAttribute? attribute = (context.ActionDescriptor.EndpointMetadata.FirstOrDefault(item => item is DynamicTransactionAttribute) as DynamicTransactionAttribute);
            if (attribute is null){await next();}
            else
            {
                DynamicDbAccessService? accessService = context.HttpContext.RequestServices
                    .GetRequiredService(attribute.DynamicDbAccessServiceType) as DynamicDbAccessService;
                if(accessService is null) { throw new InvalidOperationException($"{accessService} is not resolved yet"); }
                IDbTransaction transaction = accessService.BeginTransaction();
                await next();
                transaction.Commit();
            }
        }
    }
}
