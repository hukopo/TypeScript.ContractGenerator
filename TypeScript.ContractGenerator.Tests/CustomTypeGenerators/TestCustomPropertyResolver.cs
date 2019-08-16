using System;
using System.Reflection;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Extensions;
using SkbKontur.TypeScript.ContractGenerator.Tests.Types;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;
using SkbKontur.TypeScript.ContractGenerator.Types;

using PropertyInfo = SkbKontur.TypeScript.ContractGenerator.Types.PropertyInfo;
using TypeInfo = SkbKontur.TypeScript.ContractGenerator.Types.TypeInfo;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.CustomTypeGenerators
{
    public class TestCustomPropertyResolver : ICustomTypeGenerator
    {
        public string GetTypeLocation(ITypeInfo type)
        {
            return "";
        }

        public ITypeBuildingContext ResolveType(string initialUnitPath, ITypeInfo type, ITypeScriptUnitFactory unitFactory)
        {
            return null;
        }

        public TypeScriptTypeMemberDeclaration ResolveProperty(TypeScriptUnit unit, ITypeGenerator typeGenerator, ITypeInfo type, IPropertyInfo property)
        {
            if (!(type is TypeInfo typeInfo) || !(property is PropertyInfo propertyInfo))
                throw new NotSupportedException();

            var t = typeInfo.Type;
            var p = propertyInfo.Property;
            if (t == typeof(EnumWithConstGetterContainingRootType) && p.PropertyType.IsEnum && !p.CanWrite)
            {
                return new TypeScriptTypeMemberDeclaration
                    {
                        Name = p.Name.ToLowerCamelCase(),
                        Optional = false,
                        Type = new TypeScriptStringLiteralType(p.GetMethod.Invoke(Activator.CreateInstance(t), null).ToString()),
                    };
            }
            return null;
        }
    }
}