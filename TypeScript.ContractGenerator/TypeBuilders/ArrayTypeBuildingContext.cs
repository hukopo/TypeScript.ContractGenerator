using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Types;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class ArrayTypeBuildingContext : ITypeBuildingContext
    {
        public ArrayTypeBuildingContext([NotNull] ITypeInfo arrayType,
                                        [CanBeNull] IPropertyInfo customAttributeProvider,
                                        [NotNull] TypeScriptGenerationOptions options)
        {
            elementType = GetElementType(arrayType);
            this.customAttributeProvider = customAttributeProvider;
            this.options = options;
        }

        [NotNull]
        private ITypeInfo GetElementType([NotNull] ITypeInfo arrayType)
        {
            if (arrayType.IsArray)
                return arrayType.GetElementType() ?? throw new ArgumentNullException($"Array type's {arrayType.Name} element type is not defined");

            if (arrayType.IsGenericType && arrayType.GetGenericTypeDefinition().Equals(TypeInfo.FromType(typeof(List<>))))
                return arrayType.GetGenericArguments()[0];

            throw new ArgumentException("arrayType should be either Array or List<T>", nameof(arrayType));
        }

        public static bool Accept(ITypeInfo type)
        {
            return type.IsArray || type.IsGenericType && type.GetGenericTypeDefinition().Equals(TypeInfo.FromType(typeof(List<>)));
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
            var itemType = typeGenerator.ResolveType(elementType).ReferenceFrom(targetUnit, typeGenerator);
            var resultType = TypeScriptGeneratorHelpers.BuildTargetNullableTypeByOptions(itemType, CanItemBeNull(), options);
            return new TypeScriptArrayType(resultType);
        }

        private bool CanItemBeNull()
        {
            if (elementType.IsValueType || elementType.IsEnum || customAttributeProvider == null)
                return false;
            return options.NullabilityMode == NullabilityMode.Pessimistic
                       ? !customAttributeProvider.IsNameDefined(AnnotationsNames.ItemNotNull)
                       : customAttributeProvider.IsNameDefined(AnnotationsNames.ItemCanBeNull);
        }

        [CanBeNull]
        private readonly IPropertyInfo customAttributeProvider;

        [NotNull]
        private readonly TypeScriptGenerationOptions options;

        [NotNull]
        private readonly ITypeInfo elementType;
    }
}