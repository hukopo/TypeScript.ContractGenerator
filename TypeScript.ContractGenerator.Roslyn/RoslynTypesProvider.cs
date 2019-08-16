using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;

using SkbKontur.TypeScript.ContractGenerator;
using SkbKontur.TypeScript.ContractGenerator.Types;

namespace TypeScript.ContractGenerator.Roslyn
{
    public class RoslynTypesProvider : IRootTypesProvider
    {
        private readonly Solution solution;
        private readonly Dictionary<string, Compilation> compilations;
        private readonly string[] types;

        public RoslynTypesProvider(Solution solution, string[] types)
        {
            this.solution = solution;
            this.types = types;
            compilations = solution.Projects.ToDictionary(x => x.Name, x => x.GetCompilationAsync().Result);
        }

        public ITypeInfo[] GetRootTypes()
        {
            return types.Select(x => compilations.Select(y => y.Value.GetTypeByMetadataName(x)).First(y => y != null))
                        .Select(x => (ITypeInfo)new RoslynTypeInfo(x))
                        .ToArray();
        }

        public ITypeInfo GetType(string name)
        {
            throw new System.NotImplementedException();
        }

        public ITypeInfo[] GetTypes()
        {
            throw new System.NotImplementedException();
        }
    }
}