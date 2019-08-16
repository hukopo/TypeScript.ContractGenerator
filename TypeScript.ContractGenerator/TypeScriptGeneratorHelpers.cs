using System;

using JetBrains.Annotations;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Extensions;
using SkbKontur.TypeScript.ContractGenerator.Types;

namespace SkbKontur.TypeScript.ContractGenerator
{
    public static class TypeScriptGeneratorHelpers
    {
        public static (bool, ITypeInfo) ProcessNullable(IAttributeProvider attributeContainer, ITypeInfo type, NullabilityMode nullabilityMode)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(TypeInfo.FromType(typeof(Nullable<>))))
            {
                var underlyingType = type.GetGenericArguments()[0];
                return (true, underlyingType);
            }

            if (attributeContainer == null || !type.IsClass && !type.IsInterface)
                return (false, type);

            return (CanBeNull(attributeContainer, nullabilityMode), type);
        }

        private static bool CanBeNull([NotNull] IAttributeProvider attributeContainer, NullabilityMode nullabilityMode)
        {
            return nullabilityMode == NullabilityMode.Pessimistic
                       ? !attributeContainer.IsNameDefined(AnnotationsNames.NotNull) && !attributeContainer.IsNameDefined(AnnotationsNames.Required)
                       : attributeContainer.IsNameDefined(AnnotationsNames.CanBeNull);
        }

        [NotNull]
        public static TypeScriptType BuildTargetNullableTypeByOptions([NotNull] TypeScriptType innerType, bool isNullable, [NotNull] TypeScriptGenerationOptions options)
        {
            if (!(innerType is INullabilityWrapperType) && isNullable && options.EnableExplicitNullability)
            {
                if (!options.UseGlobalNullable)
                    return new TypeScriptOrNullType(innerType);

                if (options.UseGlobalNullable)
                    return new TypeScriptNullableType(innerType);
            }

            return innerType;
        }
    }
}