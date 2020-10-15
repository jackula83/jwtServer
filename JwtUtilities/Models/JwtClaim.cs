using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JwtUtilties.Models
{
   public class JwtClaim
   {
      public string ClaimType { get; set; }
      public string ClaimValue { get; set; }

      public static implicit operator JwtClaim(Claim a_claim)
      {
         return new JwtClaim
         {
            ClaimType = a_claim.Type,
            ClaimValue = a_claim.Value
         };
      }
   }
}
