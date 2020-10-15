using JwtUtilities.Exceptions;
using JwtUtilties.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JwtUtilities.Middleware
{
   public class AccessTokenValidator
   {
      private readonly RequestDelegate _next;

      public AccessTokenValidator(RequestDelegate next)
      {
         _next = next;
      }

      public async Task Invoke(HttpContext httpContext)
      {
         string tokenSecret = httpContext.Request.Headers[EnvironmentVariables.ACCESS_TOKEN_ID.Value].ToString();

         if (tokenSecret != EnvironmentVariables.ACCESS_TOKEN_KEY.Value)
            throw new HttpStatusException(HttpStatusException.MSG_INVALID_TOKEN, HttpStatusCode.Unauthorized);

         await _next.Invoke(httpContext);
      }
   }
}
