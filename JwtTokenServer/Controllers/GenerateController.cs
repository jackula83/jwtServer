using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JwtUtilties.Helpers;
using JwtUtilties.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

namespace JwtTokenServer.Controllers
{
   [Route("[controller]")]
   [ApiController]
   public class GenerateController : ControllerBase
   {
#if DEBUG
      /// <summary>
      /// Helps to ensure the API is hooked up correctly
      /// </summary>
      /// <returns></returns>
      [HttpGet]
      public async Task<string> Get()
      {
         return await Task.Run(() => "hello i am " + nameof(GenerateController));
      }
#endif

      /// <summary>
      /// Generates a JWT token
      /// </summary>
      /// <param name="a_payload">list of claims to insert into JWT</param>
      /// <returns>JWT token string</returns>
      [HttpPost]
      public async Task<ActionResult> Post([FromBody] List<JwtClaim> a_payload)
      {
         try
         {
            string token = await Task.Run(() => JwtUtils.GenerateToken(a_payload));
            return Content(JsonConvert.SerializeObject(token), Application.Json);
         }
         catch (Exception ex)
         {
            return BadRequest(ex);
         }
      }
   }
}
