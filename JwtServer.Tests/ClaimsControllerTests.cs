using JwtQueryServer.Controllers;
using JwtTokenServer.Controllers;
using JwtUtilties.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace JwtServer.Tests
{
   public class ClaimsControllerTests : BaseTest
   {
      private ITestOutputHelper m_logger = default;
      private Dictionary<string, string> m_testData = default;

      public ClaimsControllerTests(ITestOutputHelper a_testOutputHelper)
         : base(a_testOutputHelper) { }

      /// <summary>
      /// Generates a token and then retrieves the claims from it
      /// </summary>
      [Fact]
      public void TestClaims_Valid()
      {
         try
         {
            // generate the token
            GenerateController generate = new GenerateController();
            ClaimsController claims = new ClaimsController();

            var generateFunc = new TestDelegate<List<JwtClaim>>(generate.Post);
            var generateTask = Task.Run(() => this.RunTestAsync(generateFunc));
            generateTask.Wait();

            string tokenResult = JsonConvert.DeserializeObject<string>((generateTask.Result as ContentResult).Content);

            Assert.NotNull(tokenResult);

            // get the claims from the token
            var claimsFunc = new TestDelegate<string>(claims.Post);
            var claimsTask = Task.Run(() => this.RunTestAsync(claimsFunc, JsonConvert.SerializeObject(tokenResult)));
            claimsTask.Wait();

            var result = claimsTask.Result as ContentResult;

            Assert.NotNull(result);

            // ensure the claims match
            List<JwtClaim> claimList1 = JsonConvert.DeserializeObject<List<JwtClaim>>(result.Content);
            List<JwtClaim> claimList2 = this.GetTestData<List<JwtClaim>>(nameof(TestClaims_Valid));

            Assert.Equal(claimList1.Count, claimList2.Count);

            for (int i = 0; i < claimList1.Count; ++i)
            {
               Assert.Equal(claimList1[i].ClaimType, claimList2[i].ClaimType);
               Assert.Equal(claimList1[i].ClaimValue, claimList2[i].ClaimValue);
            }
         }
         catch (Exception e)
         {
            m_logger.WriteLine(e.Message);
            Assert.Null(e);
         }
      }

      /// <summary>
      /// Feeds an invalid token and ensures the <see cref="ClaimsController" /> provides a null
      /// </summary>
      [Fact]
      public void TestClaims_Invalid()
      {

      }

      #region implementations
      protected override string BuildTestData(string a_methodName)
      {
         if (a_methodName == nameof(TestClaims_Valid))
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

         else if (a_methodName == nameof(TestClaims_Invalid))
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
