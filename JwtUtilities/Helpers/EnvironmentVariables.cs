using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtUtilties.Helpers
{
   public class EnvironmentVariables
   {
      public static Lazy<string> ACCESS_TOKEN_ID = new Lazy<string>(() =>

#if DEBUG
            "PublicAuthToken"
#else
            Environment.GetEnvironmentVariable("AUTH_ACCESS_TOKEN")
#endif
            );

      public static Lazy<string> ACCESS_TOKEN_KEY = new Lazy<string>(() =>

#if DEBUG
            "PublicSecret"
#else
            Environment.GetEnvironmentVariable("AUTH_ACCESS_TOKEN_SECRET")
#endif
            );

      public class JWT
      {
         /// <summary>
         /// Must be at least 16 characters long
         /// </summary>
         public static Lazy<string> SECRET_KEY = new Lazy<string>(() =>

#if DEBUG
            "PublicSecretKey="
#else
            Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
#endif
            );

         public static Lazy<string> ISSUER = new Lazy<string>(() =>

#if DEBUG
            "PublicIssuer"
#else
            Environment.GetEnvironmentVariable("JWT_ISSUER")
#endif
            );

         public static Lazy<string> AUDIENCE = new Lazy<string>(() =>

#if DEBUG
            "PublicAudience"
#else
            Environment.GetEnvironmentVariable("JWT_AUDIENCE")
#endif
            );

         public static Lazy<int> TIMEOUT = new Lazy<int>(() =>

#if DEBUG
            30
#else
            int.Parse(Environment.GetEnvironmentVariable("JWT_TIMEOUT"))
#endif
            );
      }

      public class AWS
      {
         public static Lazy<string> INVOKE_ACCESS_KEY = new Lazy<string>(() =>

#if DEBUG
            "PublicAccessKeyID"
#else
            Environment.GetEnvironmentVariable("INVOKE_ACCESS_KEY")
#endif
            );

         public static Lazy<string> INVOKE_SECRET_KEY = new Lazy<string>(() =>

#if DEBUG
            "PublicSecretKeyID"
#else
            Environment.GetEnvironmentVariable("INVOKE_SECRET_KEY")
#endif
            );
      }
   }
}
