// using System.Collections;
// using System.Reflection;
// using Pilot.Contracts.Base;
//
// namespace Pilot.Tests.IntegrationBase;
//
// public class AllModelCombinationData : IEnumerable<object[]>
// {
//     public IEnumerator<object[]> GetEnumerator()
//     {
//         var baseModelType = typeof(BaseModel);
//         var assembly = Assembly.GetAssembly(baseModelType);
//
//         var modelTypes = assembly?.GetTypes()
//             .Where(t => t is { IsClass: true, IsAbstract: false } && t.IsSubclassOf(baseModelType))
//             .Select(c => new object[] { c })
//             .ToList();
//         
//         foreach (var modelType in modelTypes)
//         {
//             yield return new object[] { modelType };
//         }
//     }
//     
//
//     IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
// }