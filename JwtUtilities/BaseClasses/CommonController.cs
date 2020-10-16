using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace JwtUtilities.BaseClasses
{
   public abstract class CommonController : ControllerBase
   {
      protected ContentResult JsonContent(object a_content)
      {
         return Content(JsonConvert.SerializeObject(a_content), Application.Json);
      }

      protected async Task<ActionResult> RunAsync<TResultType>(Func<TResultType> a_func)
      {
         try
         {
            TResultType result = await Task.Run(() => a_func());
            return JsonContent(result);
         }
         catch (Exception ex)
         {
            return BadRequest(ex);
         }
      }
   }
}
