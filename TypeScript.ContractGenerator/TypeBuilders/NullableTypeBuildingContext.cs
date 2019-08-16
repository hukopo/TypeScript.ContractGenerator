using System;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Types;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class NullableTypeBuildingContext : ITypeBuildingContext
    {
        public NullableTypeBuildingContext(ITypeInfo nullableUnderlyingType, bool useGlobalNullable)
        {
            itemType = nullableUnderlyingType;
            this.useGlobalNullable = useGlobalNullable;
        }

        public bool IsDefinitionBuilt => true;

        public void Initialize(ITypeGenerator typeGenerator)
        {
        }

        public void BuildDefinition(ITypeGenerator typeGenerator)
        {
        }

        public TypeScriptType ReferenceFrom(TypeScriptUnit targetUnit, ITypeGenerator typeGenerator)
        {
            var itemTypeScriptType = typeGenerator.ResolveType(itemType).ReferenceFrom(targetUnit, typeGenerator);
            return useGlobalNullable
                       ? (TypeScriptType)new TypeScriptNullableType(itemTypeScriptType)
                       : new TypeScriptOrNullType(itemTypeScriptType);
        }

        private readonly ITypeInfo itemType;
        private readonly bool useGlobalNullable;
    }
}