using System;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Types;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class GenericParameterTypeBuildingContext : ITypeBuildingContext
    {
        public GenericParameterTypeBuildingContext(ITypeInfo type)
        {
            this.type = type;
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
            return new TypeScriptTypeReference(type.Name);
        }

        private readonly ITypeInfo type;
    }
}