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

namespace JwtServer.Tests
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
            GenerateController generate = new GenerateController();

            var generateFunc = new TestDelegate<List<JwtClaim>>(generate.Post);
            var generateTask = Task.Run(() => this.RunTestAsync(generateFunc));
            generateTask.Wait();

            var result = generateTask.Result as ContentResult;
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
            GenerateController generate = new GenerateController();

            var generateFunc = new TestDelegate<List<JwtClaim>>(generate.Post);
            var generateTask = Task.Run(() => this.RunTestAsync(generateFunc));
            generateTask.Wait();

            var result = generateTask.Result as BadRequestObjectResult;

            Assert.NotNull(result);
         }
         catch (Exception e)
         {
            m_logger.WriteLine(e.Message);
            Assert.Null(e);
         }
      }

      #region implementations
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
