using JwtServer.Tests;
using JwtTokenServer.Controllers;
using JwtUtilties.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace JwtTokenServer.Tests
{
   public class GenerateControllerTests : BaseTest
   {
      private ITestOutputHelper m_logger = default;
      private Dictionary<string, string> m_testData = default;

      public GenerateControllerTests(ITestOutputHelper a_testOutputHelper)
         : base(a_testOutputHelper) { }

      /// <summary>
      /// Tests with a valid token
      /// </summary>
      [Fact]
      public void TestGenerateToken_Valid()
      {
         try
         {
            GenerateController controller = new GenerateController();

            MethodBase method = MethodInfo.GetCurrentMethod();

            var task = Task.Run(() => controller.Post(this.GetTestData<List<JwtClaim>>(method.Name)));
            task.Wait();

            var result = task.Result as ContentResult;
            var token = JsonConvert.DeserializeObject<string>(result.Content);

            Assert.NotNull(token);
         }
         catch (Exception e)
         {
            m_logger.WriteLine(e.Message);
            Assert.Null(e);
         }
      }

      /// <summary>
      /// Tests with an invalid token, ensure 
      /// </summary>
      [Fact]
      public void TestGenerateToken_Invalid()
      {
         try
         {
            GenerateController controller = new GenerateController();

            MethodBase method = MethodInfo.GetCurrentMethod();

            var task = Task.Run(() => controller.Post(this.GetTestData<List<JwtClaim>>(method.Name)));
            task.Wait();

            var result = task.Result as BadRequestObjectResult;

            Assert.NotNull(result);
         }
         catch (Exception e)
         {
            m_logger.WriteLine(e.Message);
            Assert.Null(e);
         }
      }

      #region implementations
      /// <summary>
      /// Build the test data here, away from test routine to unclutter the code
      /// </summary>
      /// <param name="a_methodName">the name of the method to build test data for</param>
      /// <returns>generally a json string containing the body of the request</returns>
      protected override string BuildTestData(string a_methodName)
      {
         if (a_methodName == nameof(TestGenerateToken_Valid))
         {
            List<string> jsonBuilder = new List<string>
            {
               this.BuildJsonString(new List<dynamic>
               {
                  new {First = "claimType", Second = "userName" },
                  new {First = "claimValue", Second = "myUsername"}
               }),
               this.BuildJsonString(new List<dynamic>
               {
                  new {First = "claimType", Second = "firstName" },
                  new {First = "claimValue", Second = "Jack"}
               }),
               this.BuildJsonString(new List<dynamic>
               {
                  new {First = "claimType", Second = "emailAddress" },
                  new {First = "claimValue", Second = "jack@adsf.com"}
               })
            };

            return "[" + string.Join(',', jsonBuilder.ToArray()) + "]";
         }

         else if (a_methodName == nameof(TestGenerateToken_Invalid))
         {
            List<string> jsonBuilder = new List<string>
            {
               this.BuildJsonString(new List<dynamic>{new {First = "invalid", Second = "invalid" }})
            };

            return "[" + string.Join(',', jsonBuilder.ToArray()) + "]";
         }

         return default;
      }
      #endregion // implementations
   }
}
