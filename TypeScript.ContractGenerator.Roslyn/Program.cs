using System;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

using SkbKontur.TypeScript.ContractGenerator;

namespace TypeScript.ContractGenerator.Roslyn
{
    class Program
    {
        static void Main(string[] args)
        {
            var w = MSBuildWorkspace.Create();
            var sln = w.OpenSolutionAsync("../TypeScript.ContractGenerator.sln").Result;
            var generator = new TypeScriptGenerator(TypeScriptGenerationOptions.Default, CustomTypeGenerator.Null, new RoslynTypesProvider(sln, new []
                {
                    "SkbKontur.TypeScript.ContractGenerator.Tests.Types.ArrayRootType"
                }));
            generator.Generate();
        }
    }
}