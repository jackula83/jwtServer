using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace JwtServer.Tests
{
   public abstract class BaseTest
   {
      private ITestOutputHelper m_logger = default;
      private Dictionary<string, string> m_testData = default;

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

      protected virtual T GetTestData<T>(string a_methodName)
      {
         return JsonConvert.DeserializeObject<T>(m_testData[a_methodName]);
      }

      protected virtual string BuildJsonString(List<dynamic> a_pairList)
      {
         List<string> objList = new List<string>();

         a_pairList.ForEach(x => objList.Add(string.Format("\"{0}\":\"{1}\"", x.First, x.Second)));

         return "{" + string.Join(',', objList.ToArray()) + "}";
      }

      protected abstract string BuildTestData(string a_methodName);
   }
}
