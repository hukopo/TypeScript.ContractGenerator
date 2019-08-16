using System;
using System.Reflection;

using JetBrains.Annotations;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;
using SkbKontur.TypeScript.ContractGenerator.Types;

namespace SkbKontur.TypeScript.ContractGenerator
{
    public interface ITypeGenerator
    {
        [NotNull]
        ITypeBuildingContext ResolveType([NotNull] ITypeInfo type);

        [CanBeNull]
        TypeScriptTypeMemberDeclaration ResolveProperty([NotNull] TypeScriptUnit unit, [NotNull] ITypeInfo type, [NotNull] IPropertyInfo propertyInfo);

        [NotNull]
        TypeScriptType BuildAndImportType([NotNull] TypeScriptUnit targetUnit, [CanBeNull] IPropertyInfo customAttributeProvider, [NotNull] ITypeInfo type);

        [NotNull]
        TypeScriptGenerationOptions Options { get; }
    }
}