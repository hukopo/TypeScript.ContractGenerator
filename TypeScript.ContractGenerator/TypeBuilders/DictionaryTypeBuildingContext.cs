using System;
using System.Collections.Generic;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Types;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class DictionaryTypeBuildingContext : ITypeBuildingContext
    {
        public DictionaryTypeBuildingContext(ITypeInfo dictionaryType)
        {
            keyType = dictionaryType.GetGenericArguments()[0];
            valueType = dictionaryType.GetGenericArguments()[1];
        }

        public static bool Accept(ITypeInfo type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(TypeInfo.FromType(typeof(Dictionary<,>)));
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
            var keyTypeScriptType = typeGenerator.ResolveType(keyType).ReferenceFrom(targetUnit, typeGenerator);
            var valueTypeScriptType = typeGenerator.ResolveType(valueType).ReferenceFrom(targetUnit, typeGenerator);

            return new TypeScriptTypeDefintion
                {
                    Members =
                        {
                            new TypeScriptTypePropertyGetterDeclaration
                                {
                                    Argument = new TypeScriptArgumentDeclaration
                                        {
                                            Name = "key",
                                            Type = keyTypeScriptType,
                                        },
                                    ResultType = valueTypeScriptType,
                                    Optional = true,
                                }
                        }
                };
        }

        private readonly ITypeInfo keyType;
        private readonly ITypeInfo valueType;
    }
}