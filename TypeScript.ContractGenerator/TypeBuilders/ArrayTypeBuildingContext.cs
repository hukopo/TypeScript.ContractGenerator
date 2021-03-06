using System;
using System.Collections.Generic;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Extensions;
using SkbKontur.TypeScript.ContractGenerator.Internals;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class ArrayTypeBuildingContext : ITypeBuildingContext
    {
        public ArrayTypeBuildingContext(ITypeInfo arrayType, TypeScriptGenerationOptions options)
        {
            elementType = GetElementType(arrayType);
            this.options = options;
        }

        private ITypeInfo GetElementType(ITypeInfo arrayType)
        {
            if (arrayType.IsArray)
                return arrayType.GetElementType() ?? throw new ArgumentNullException($"Array type's {arrayType.Name} element type is not defined");

            if (arrayType.IsGenericType && arrayType.GetGenericTypeDefinition().Equals(TypeInfo.From(typeof(List<>))))
                return arrayType.GetGenericArguments()[0];

            throw new ArgumentException("arrayType should be either Array or List<T>", nameof(arrayType));
        }

        public static bool Accept(ITypeInfo type)
        {
            return type.IsArray || type.IsGenericType && type.GetGenericTypeDefinition().Equals(TypeInfo.From(typeof(List<>)));
        }

        public bool IsDefinitionBuilt => true;

        public void Initialize(ITypeGenerator typeGenerator)
        {
        }

        public void BuildDefinition(ITypeGenerator typeGenerator)
        {
        }

        public TypeScriptType ReferenceFrom(TypeScriptUnit targetUnit, ITypeGenerator typeGenerator, IAttributeProvider? attributeProvider)
        {
            var itemType = typeGenerator.ResolveType(elementType).ReferenceFrom(targetUnit, typeGenerator, null);
            var resultType = TypeScriptGeneratorHelpers.BuildTargetNullableTypeByOptions(itemType, CanItemBeNull(attributeProvider), options);
            return new TypeScriptArrayType(resultType);
        }

        private bool CanItemBeNull(IAttributeProvider? attributeProvider)
        {
            if (elementType.IsValueType || elementType.IsEnum || attributeProvider == null)
                return false;

            if (options.NullabilityMode == NullabilityMode.NullableReference)
                return TypeScriptGeneratorHelpers.NullableReferenceCanBeNull(attributeProvider, elementType, 1);

            return options.NullabilityMode == NullabilityMode.Pessimistic
                       ? !attributeProvider.IsNameDefined(AnnotationsNames.ItemNotNull)
                       : attributeProvider.IsNameDefined(AnnotationsNames.ItemCanBeNull);
        }

        private readonly TypeScriptGenerationOptions options;
        private readonly ITypeInfo elementType;
    }
}