using System.Threading.Tasks;
using JwtUtilties.Helpers;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JwtQueryServer.Controllers
{
   [Route("[controller]")]
   [ApiController]
   public class ValidateController : ControllerBase
   {
#if DEBUG
      /// <summary>
      /// Helps to ensure the API is hooked up correctly
      /// </summary>
      /// <returns></returns>
      [HttpGet]
      public async Task<string> Get()
      {
         return await Task.Run(() => "hello i am " + nameof(ValidateController));
      }
#endif

      /// <summary>
      /// Validates the given token
      /// </summary>
      /// <param name="a_token">token string, quote-less</param>
      /// <returns>true if successfully validated, false otherwise</returns>
      [HttpPost]
      public async Task<bool> Post([FromBody] string a_token)
      {
         return await Task.Run(() => JwtUtils.ValidateToken(a_token)) != default;
      }
   }
}
