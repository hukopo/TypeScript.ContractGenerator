using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Internals;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;
using SkbKontur.TypeScript.ContractGenerator.Types;

namespace SkbKontur.TypeScript.ContractGenerator
{
    public class CustomTypeGenerator : ICustomTypeGenerator
    {
        public virtual string GetTypeLocation(ITypeInfo type)
        {
            if (typeLocations.TryGetValue(type, out var getLocation))
                return getLocation(type);
            foreach(var typeLocationRule in typeLocationRules)
                if (typeLocationRule.Accept(type))
                    return typeLocationRule.GetLocation(type);
            return string.Empty;
        }

        public virtual ITypeBuildingContext ResolveType(string initialUnitPath, ITypeInfo type, ITypeScriptUnitFactory unitFactory)
        {
            if (typeRedirects.TryGetValue(type, out var redirect))
                return TypeBuilding.RedirectToType(redirect.Name, redirect.Location, type);
            if (typeBuildingContexts.TryGetValue(type, out var createContext))
                return createContext(type);
            foreach(var typeBuildingContextWithAcceptanceChecking in typeBuildingContextsWithAcceptanceChecking)
                if (typeBuildingContextWithAcceptanceChecking.Accept(type))
                    return typeBuildingContextWithAcceptanceChecking.CreateContext(unitFactory.GetOrCreateTypeUnit(initialUnitPath), type);
            return null;
        }

        public virtual TypeScriptTypeMemberDeclaration ResolveProperty(TypeScriptUnit unit, ITypeGenerator typeGenerator, ITypeInfo type, IPropertyInfo property)
        {
            foreach (var propertyResolver in propertyResolvers)
            {
                var result = propertyResolver.ResolveProperty(unit, typeGenerator, type, property);
                if(result != null)
                    return result;
            }
            return null;
        }

        public CustomTypeGenerator WithTypeLocation<T>(Func<ITypeInfo, string> getLocation)
        {
            typeLocations[TypeInfo.FromType<T>()] = getLocation;
            return this;
        }

        public CustomTypeGenerator WithTypeLocationRule(Func<ITypeInfo, bool> accept, Func<ITypeInfo, string> getLocation)
        {
            typeLocationRules.Add((accept, getLocation));
            return this;
        }

        public CustomTypeGenerator WithTypeRedirect<T>(string name, string location)
        {
            typeRedirects[TypeInfo.FromType<T>()] = new TypeLocation {Name = name, Location = location};
            return this;
        }

        public CustomTypeGenerator WithTypeBuildingContext<T>(Func<ITypeInfo, ITypeBuildingContext> createContext)
        {
            typeBuildingContexts[TypeInfo.FromType<T>()] = createContext;
            return this;
        }

        public CustomTypeGenerator WithTypeBuildingContext(Func<ITypeInfo, bool> accept, Func<TypeScriptUnit, ITypeInfo, ITypeBuildingContext> createContext)
        {
            typeBuildingContextsWithAcceptanceChecking.Add((accept, createContext));
            return this;
        }

        public CustomTypeGenerator WithPropertyResolver(IPropertyResolver propertyResolver)
        {
            propertyResolvers.Add(propertyResolver);
            return this;
        }

        [NotNull]
        public static ICustomTypeGenerator Null => new NullCustomTypeGenerator();

        private readonly Dictionary<ITypeInfo, Func<ITypeInfo, string>> typeLocations = new Dictionary<ITypeInfo, Func<ITypeInfo, string>>();
        private readonly Dictionary<ITypeInfo, TypeLocation> typeRedirects = new Dictionary<ITypeInfo, TypeLocation>();
        private readonly Dictionary<ITypeInfo, Func<ITypeInfo, ITypeBuildingContext>> typeBuildingContexts = new Dictionary<ITypeInfo, Func<ITypeInfo, ITypeBuildingContext>>();
        private readonly List<(Func<ITypeInfo, bool> Accept, Func<TypeScriptUnit, ITypeInfo, ITypeBuildingContext> CreateContext)> typeBuildingContextsWithAcceptanceChecking = new List<(Func<ITypeInfo, bool> Accept, Func<TypeScriptUnit, ITypeInfo, ITypeBuildingContext> CreateContext)>();
        private readonly List<(Func<ITypeInfo, bool> Accept, Func<ITypeInfo, string> GetLocation)> typeLocationRules = new List<(Func<ITypeInfo, bool> Accept, Func<ITypeInfo, string> GetLocation)>();
        private readonly List<IPropertyResolver> propertyResolvers = new List<IPropertyResolver>();
    }
}