using JwtUtilties.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace JwtUtilties.Helpers
{
   public class JwtUtils
   {
      private static Lazy<SymmetricSecurityKey> m_securityKey =
         new Lazy<SymmetricSecurityKey>(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(EnvironmentVariables.JWT.SECRET_KEY.Value)));

      private static Lazy<SigningCredentials> m_credentials = new Lazy<SigningCredentials>(
         new SigningCredentials(m_securityKey.Value, SecurityAlgorithms.HmacSha256));

      /// <summary>
      /// Generates a JWT token with the provided claims.
      /// </summary>
      /// <param name="a_payload">array of {type, value}</param>
      /// <returns></returns>
      public static string GenerateToken(List<JwtClaim> a_payload)
      {
         a_payload.RemoveAll(c => c.ClaimType == JwtRegisteredClaimNames.Iss);
         a_payload.RemoveAll(c => c.ClaimType == JwtRegisteredClaimNames.Aud);
         a_payload.RemoveAll(c => c.ClaimType == JwtRegisteredClaimNames.Exp);

         JwtSecurityToken token = new JwtSecurityToken(
            issuer: EnvironmentVariables.JWT.ISSUER.Value,
            audience: EnvironmentVariables.JWT.AUDIENCE.Value,
            claims: a_payload.Select(c => new Claim(c.ClaimType, c.ClaimValue)),
            expires: DateTime.Now.AddMinutes(EnvironmentVariables.JWT.TIMEOUT.Value),
            signingCredentials: m_credentials.Value
         );

         return new JwtSecurityTokenHandler().WriteToken(token);
      }

      /// <summary>
      /// validates a JWT token
      /// </summary>
      /// <param name="a_token">the JWT token, without quotes</param>
      /// <returns>the <see cref="ClaimsPrincipal"/> if successfully validated, null otherwise</returns>
      public static ClaimsPrincipal ValidateToken(string a_token)
      {
         // These need to match the values used to generate the token
         TokenValidationParameters validationParameters = new TokenValidationParameters
         {
            ValidIssuer = EnvironmentVariables.JWT.ISSUER.Value,
            ValidAudience = EnvironmentVariables.JWT.AUDIENCE.Value,
            IssuerSigningKey = m_securityKey.Value,
            ValidateIssuerSigningKey = true,
            ValidateAudience = true
         };

         JwtSecurityTokenHandler validator = new JwtSecurityTokenHandler();
         if (validator.CanReadToken(a_token))
         {
            ClaimsPrincipal principal = default;
            try
            {
               SecurityToken validatedToken = default;

               // this line throws if invalid
               principal = validator.ValidateToken(a_token, validationParameters, out validatedToken);

               // If we got here then the token is valid
               return principal;
            }
            catch (Exception)
            {
               // TBD at some stage
            }
         }

         return default;
      }

      /// <summary>
      /// Gets the claims from a JWT token
      /// </summary>
      /// <param name="a_token">the JWT token, without quotes</param>
      /// <returns>a list of claim types and their values, null if cannot be validated</returns>
      public static List<JwtClaim> Get(string a_token)
      {
         var principal = ValidateToken(a_token);

         if (principal == default)
            return default;

         // else
         List<JwtClaim> results = new List<JwtClaim>();

         try
         {
            foreach (Claim claim in principal.Claims)
               results.Add(claim);

            // remove built-in claims
            results.RemoveAll(c => c.ClaimType == JwtRegisteredClaimNames.Iss);
            results.RemoveAll(c => c.ClaimType == JwtRegisteredClaimNames.Aud);
            results.RemoveAll(c => c.ClaimType == JwtRegisteredClaimNames.Exp);
         }
         catch (Exception)
         {
            // on error return nothing
            results = default;
         }

         return results;
      }
   }
}
