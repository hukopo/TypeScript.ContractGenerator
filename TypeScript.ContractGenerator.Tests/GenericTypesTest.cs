using FluentAssertions;

using NUnit.Framework;

using SkbKontur.TypeScript.ContractGenerator.Tests.Types;
using SkbKontur.TypeScript.ContractGenerator.Types;

namespace SkbKontur.TypeScript.ContractGenerator.Tests
{
    public class GenericTypesTest
    {
        [Test]
        public void RootCantBeGenericType()
        {
            new TypeScriptGenerator(TypeScriptGenerationOptions.Default, CustomTypeGenerator.Null, new RootTypesProvider(typeof(GenericRootType<CustomType>)))
                .Generate().Should().BeEmpty();
        }
    }
}