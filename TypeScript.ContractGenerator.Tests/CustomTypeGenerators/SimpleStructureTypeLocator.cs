using System;
using System.Reflection;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;
using SkbKontur.TypeScript.ContractGenerator.Types;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.CustomTypeGenerators
{
    public class SimpleStructureTypeLocator : ICustomTypeGenerator
    {
        public string GetTypeLocation(ITypeInfo type)
        {
            return $"{type.Name}\\{type.Name}";
        }

        public ITypeBuildingContext ResolveType(string initialUnitPath, ITypeInfo type, ITypeScriptUnitFactory unitFactory)
        {
            return null;
        }

        public TypeScriptTypeMemberDeclaration ResolveProperty(TypeScriptUnit unit, ITypeGenerator typeGenerator, ITypeInfo type, IPropertyInfo property)
        {
            return null;
        }
    }
}