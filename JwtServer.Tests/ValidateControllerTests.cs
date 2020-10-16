using JwtQueryServer.Controllers;
using JwtTokenServer.Controllers;
using JwtUtilties.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace JwtServer.Tests
{
   public class ValidateControllerTests : BaseTest
   {
      private ITestOutputHelper m_logger = default;
      private Dictionary<string, string> m_testData = default;

      public ValidateControllerTests(ITestOutputHelper a_testOutputHelper)
         : base(a_testOutputHelper) { }

      // ensures validation returns True on valid token
      [Fact]
      public void TestValidateToken_ValidTrue()
      {
         try
         {
            GenerateController generate = new GenerateController();
            ValidateController validate = new ValidateController();

            // generate a token then validate it
            var generateFunc = new TestDelegate<List<JwtClaim>>(generate.Post);
            var generateTask = Task.Run(() => this.RunTestAsync(generateFunc));
            generateTask.Wait();

            string tokenResult = JsonConvert.DeserializeObject<string>((generateTask.Result as ContentResult).Content);

            Assert.NotNull(tokenResult);

            // token validation part
            var validateFunc = new TestDelegate<string>(validate.Post);
            var validateTask = Task.Run(() => this.RunTestAsync(validateFunc, JsonConvert.SerializeObject(tokenResult)));
            validateTask.Wait();

            var result = validateTask.Result as ContentResult;
            var validated = JsonConvert.DeserializeObject<bool>((validateTask.Result as ContentResult).Content);

            Assert.True(validated);
         }
         catch (Exception e)
         {
            m_logger.WriteLine(e.Message);
            Assert.Null(e);
         }
      }

      // ensures validation returns False on invalid token
      [Fact]
      public void TestValidateToken_ValidFalse()
      {
         try
         {
            ValidateController validate = new ValidateController();

            var validateFunc = new TestDelegate<string>(validate.Post);
            var validateTask = Task.Run(() => this.RunTestAsync(validateFunc));
            validateTask.Wait();

            var result = validateTask.Result as ContentResult;
            var validated = JsonConvert.DeserializeObject<bool>((validateTask.Result as ContentResult).Content);

            Assert.False(validated);
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
         if (a_methodName == nameof(TestValidateToken_ValidTrue))
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

         else if (a_methodName == nameof(TestValidateToken_ValidFalse))
         {
            // default token taken from jwt.io
            return JsonConvert.SerializeObject(
               "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c");
         }

         return default;
      }
      #endregion // implementations
   }
}
