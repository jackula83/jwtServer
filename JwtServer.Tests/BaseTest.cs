﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace JwtServer.Tests
{
   public abstract class BaseTest
   {
      private ITestOutputHelper m_logger = default;
      private Dictionary<string, string> m_testData = default;

      protected delegate Task<ActionResult> TestDelegate<TInputType>(TInputType a_input);

      public BaseTest(ITestOutputHelper a_testOutputHelper)
      {
         m_logger = a_testOutputHelper;
         m_testData = new Dictionary<string, string>();

         MethodInfo[] methods = Assembly.GetExecutingAssembly().GetTypes()
            .SelectMany(t => t.GetMethods())
            .Where(m => m.GetCustomAttributes(typeof(FactAttribute), false).Length > 0)
            .ToArray();

         foreach (MethodInfo method in methods)
            m_testData[method.Name] = this.BuildTestData(method.Name);
      }

      /// <summary>
      /// Runs a function to test, providing the default test data unless override is provided
      /// </summary>
      /// <typeparam name="TInputType">the type of input required by the function being tested</typeparam>
      /// <param name="a_func">the delegate for the function being tested</param>
      /// <param name="a_testDataOverride">if null then will use the default test data initialised in <see cref="BuildTestData" />, otherwise the json provided will be used instead</param>
      /// <returns>the Action result as returned by the function being tested</returns>
      protected virtual async Task<ActionResult> RunTestAsync<TInputType>(TestDelegate<TInputType> a_func, string a_testDataOverride = null, [CallerMemberName] string a_callingMethod = null)
      {
         Assert.NotNull(a_callingMethod);

         // calling method name
         StackTrace stackTrace = new StackTrace();

         // get the test data
         TInputType testData = this.GetTestData<TInputType>(a_callingMethod, a_testDataOverride);

         // invoke and return results
         return await Task.Run(() => a_func(testData));
      }

      #region Test Data Builders
      /// <summary>
      /// Gets the test data associated with a method name
      /// </summary>
      /// <typeparam name="T">the expected type of object once deserialized from the test data json</typeparam>
      /// <param name="a_methodName">name of the method for which the test data is stored</param>
      /// <param name="a_testDataOverride">if null then will use the default test data initialised in <see cref="BuildTestData" />, otherwise the json provided will be used instead</param>
      /// <returns></returns>
      protected virtual T GetTestData<T>(string a_methodName, string a_testDataOverride = null)
      {
         return JsonConvert.DeserializeObject<T>(a_testDataOverride ?? m_testData[a_methodName]);
      }

      /// <summary>
      /// builds a json string object based on the provided list of parameters
      /// </summary>
      /// <param name="a_pairList">a list of {First: json key, Second: json value}</param>
      /// <returns>a json string in the format of {"key1":"value1",...{"keyN","valueN"}</returns>
      protected virtual string BuildJsonString(List<dynamic> a_pairList)
      {
         List<string> objList = new List<string>();

         a_pairList.ForEach(x => objList.Add(string.Format("\"{0}\":\"{1}\"", x.First, x.Second)));

         return "{" + string.Join(',', objList.ToArray()) + "}";
      }
      #endregion // Test Data Builders

      #region Abstracts
      /// <summary>
      /// Build the test data here, away from test routine to unclutter the code
      /// </summary>
      /// <param name="a_methodName">the name of the method to build test data for</param>
      /// <returns>generally a json string containing the body of the request</returns>
      protected abstract string BuildTestData(string a_methodName);
      #endregion // Abstracts
   }
}
