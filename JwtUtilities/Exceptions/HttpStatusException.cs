using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace JwtUtilities.Exceptions
{
   public class HttpStatusException : Exception
   {
      public const string MSG_INVALID_TOKEN = "Invalid access token";

      public HttpStatusCode StatusCode { get; set; }

      public HttpStatusException(string a_message, HttpStatusCode a_statusCode)
         : base(a_message)
      {
         this.StatusCode = a_statusCode;
      }
   }
}
