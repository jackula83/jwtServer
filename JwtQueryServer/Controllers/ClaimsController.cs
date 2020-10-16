using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JwtUtilities.BaseClasses;
using JwtUtilties.Helpers;
using JwtUtilties.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtQueryServer.Controllers
{
   [Route("[controller]")]
   [ApiController]
   public class ClaimsController : CommonController
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
      public async Task<ActionResult> Post([FromBody] string a_token)
      {
         return await this.RunAsync(() => JwtUtils.Get(a_token));
      }
   }
}
