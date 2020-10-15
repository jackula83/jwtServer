using System;
using System.Collections.Generic;
using System.Text;

namespace JwtUtilties.Models
{
   public class JwtRequestModel
   {
      public enum RequestTypeE : int
      {
         INVALID = 0,
         GENERATE,
         VALIDATE,
         GET
      }

      public RequestTypeE RequestType { get; set; } = RequestTypeE.INVALID;

      public List<JwtClaim> Payload { get; set; }
   }
}
