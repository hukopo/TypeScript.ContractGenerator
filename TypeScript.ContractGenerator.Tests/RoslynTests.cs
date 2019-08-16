using System;
using System.Linq;

using FluentAssertions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

using NUnit.Framework;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Tests.Types;

using TypeScript.ContractGenerator.Roslyn;

namespace SkbKontur.TypeScript.ContractGenerator.Tests
{
    public class RoslynTests : TestBase
    {
        public RoslynTests(JavaScriptTypeChecker javaScriptTypeChecker)
            : base(javaScriptTypeChecker)
        {
        }

        [TestCase(typeof(NamingRootType), "type-names")]
        [TestCase(typeof(SimpleRootType), "simple-types")]
        [TestCase(typeof(SimpleNullableRootType), "nullable-types")]
        [TestCase(typeof(EnumContainingRootType), "enum-types")]
        [TestCase(typeof(ComplexRootType), "complex-types")]
        [TestCase(typeof(GenericRootType<>), "generic-root")]
        [TestCase(typeof(GenericContainingRootType), "generic-types")]
        [TestCase(typeof(ArrayRootType), "array-types")]
        [TestCase(typeof(NotNullRootType), "notnull-types")]
        [TestCase(typeof(NonDefaultConstructorRootType), "non-default-constructor")]
        [TestCase(typeof(IgnoreRootType), "ignore-type")]
        [Ignore("Nothing works yet")]
        public void GenerateCodeTest(Type rootType, string expectedFileName)
        {
            var w = MSBuildWorkspace.Create();
            var sln = w.OpenSolutionAsync(TestContext.CurrentContext.TestDirectory + "/../../../../TypeScript.ContractGenerator.sln").Result;
            var typesProvider = new RoslynTypesProvider(sln, new[] {rootType.FullName});
            var generatedCode = GenerateCode(TypeScriptGenerationOptions.Default, CustomTypeGenerator.Null, typesProvider).Single().Replace("\r\n", "\n");
            var expectedCode = GetExpectedCode($"SimpleGenerator/{expectedFileName}");
            generatedCode.Should().Be(expectedCode);
        }
    }
}