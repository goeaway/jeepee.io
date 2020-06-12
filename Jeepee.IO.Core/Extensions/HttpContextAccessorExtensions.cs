using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Jeepee.IO.Core.Extensions
{
    public static class HttpContextAccessorExtensions
    {
        public static string GetUserIdent(this IHttpContextAccessor contextAccessor)
        {
            return contextAccessor
                .HttpContext
                .User
                .Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Email)
                ?.Value;
        }
    }
}
