using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JwtUtilties.Helpers;
using JwtUtilties.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtQueryServer.Controllers
{
   [Route("[controller]")]
   [ApiController]
   public class ClaimsController : ControllerBase
   {
#if DEBUG
      /// <summary>
      /// Helps to ensure the API is hooked up correctly
      /// </summary>
      /// <returns></returns>
      [HttpGet]
      public async Task<string> Get()
      {
         return await Task.Run(() => "hello i am " + nameof(ClaimsController));
      }
#endif

      /// <summary>
      /// Gets a list of claims from the token
      /// </summary>
      /// <param name="a_token">token string, quoteless</param>
      /// <returns>a list of tokens in format of [{"claimType":"claimValue"}...]</returns>
      [HttpPost]
      public async Task<List<JwtClaim>> Post([FromBody] string a_token)
      {
         return await Task.Run(() => JwtUtils.Get(a_token));
      }
   }
}
