using System.Linq;
using System.Reflection;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using SkbKontur.TypeScript.ContractGenerator;
using SkbKontur.TypeScript.ContractGenerator.Types;

namespace TypeScript.ContractGenerator.Roslyn
{
    public class RoslynTypeInfo : ITypeInfo
    {
        public RoslynTypeInfo(ITypeSymbol symbol)
        {
            TypeSymbol = symbol;
        }

        public ITypeSymbol TypeSymbol { get; }

        public bool IsGenericType => TypeSymbol is INamedTypeSymbol ts && ts.IsGenericType;
        public bool IsClass => TypeSymbol.TypeKind == TypeKind.Class;
        public bool IsAbstract => TypeSymbol.IsAbstract;
        public bool IsGenericTypeDefinition => TypeSymbol is INamedTypeSymbol ts && ts.IsUnboundGenericType;
        public bool IsArray => TypeSymbol.Kind == SymbolKind.ArrayType;
        public string FullName => TypeSymbol.MetadataName;
        public string Name => TypeSymbol.Name;
        public string Namespace => TypeSymbol.ContainingNamespace.Name;
        public ITypeInfo BaseType => new RoslynTypeInfo(TypeSymbol.BaseType);
        public bool IsGenericParameter => false;
        public bool IsEnum => TypeSymbol.TypeKind == TypeKind.Enum;
        public ITypeInfo[] GenericTypeArguments => TypeSymbol is INamedTypeSymbol ts ? ts.TypeArguments.Select(x => (ITypeInfo)new RoslynTypeInfo(x)).ToArray() : new ITypeInfo[0];

        public ITypeInfo GetGenericTypeDefinition()
        {
            if (!(TypeSymbol is INamedTypeSymbol ts))
                return null;
            return new RoslynTypeInfo(ts.ConstructUnboundGenericType());
        }

        public ITypeInfo[] GetGenericArguments()
        {
            return GenericTypeArguments;
        }

        public ITypeInfo[] GetInterfaces()
        {
            return TypeSymbol.Interfaces.Select(x => (ITypeInfo)new RoslynTypeInfo(x)).ToArray();
        }

        public ITypeInfo GetElementType()
        {
            throw new System.NotImplementedException();
        }

        public string[] GetEnumNames()
        {
            throw new System.NotImplementedException();
        }

        public IPropertyInfo[] GetProperties(BindingFlags bindingFlags)
        {
            throw new System.NotImplementedException();
        }

        public ITypeInfo[] GetCustomAttributes()
        {
            throw new System.NotImplementedException();
        }

        public bool Equals(ITypeInfo other)
        {
            throw new System.NotImplementedException();
        }
    }
}