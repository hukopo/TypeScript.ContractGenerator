using System;
using System.Linq;
using System.Reflection;

namespace SkbKontur.TypeScript.ContractGenerator.Types
{
    public interface IAttributeProvider
    {
        ITypeInfo[] GetCustomAttributes();
    }

    public interface ITypeInfo : IAttributeProvider, IEquatable<ITypeInfo>
    {
        bool IsGenericType { get; }
        bool IsClass { get; }
        bool IsAbstract { get; }
        bool IsGenericTypeDefinition { get; }
        bool IsArray { get; }
        string FullName { get; }
        string Name { get; }
        string Namespace { get; }
        ITypeInfo BaseType { get; }
        bool IsGenericParameter { get; }
        bool IsEnum { get; }
        ITypeInfo[] GenericTypeArguments { get; }
        ITypeInfo GetGenericTypeDefinition();
        ITypeInfo[] GetGenericArguments();
        ITypeInfo[] GetInterfaces();
        ITypeInfo GetElementType();
        string[] GetEnumNames();
        IPropertyInfo[] GetProperties(BindingFlags bindingFlags);
    }

    public interface IPropertyInfo : IAttributeProvider
    {
        string Name { get; }
        ITypeInfo PropertyType { get; }
    }

    public class PropertyInfo : IPropertyInfo
    {
        public PropertyInfo(System.Reflection.PropertyInfo propertyInfo)
        {
            Name = propertyInfo.Name;
            PropertyType = TypeInfo.FromType(propertyInfo.PropertyType);
            Property = propertyInfo;
        }

        public string Name { get; }
        public ITypeInfo PropertyType { get; }
        public System.Reflection.PropertyInfo Property { get; }

        public ITypeInfo[] GetCustomAttributes()
        {
            return Property.GetCustomAttributes().Select(x => (ITypeInfo)TypeInfo.FromType(x.GetType())).ToArray();
        }
    }

    public class TypeInfo : ITypeInfo
    {
        public static TypeInfo FromType<T>()
        {
            return new TypeInfo(typeof(T));
        }

        public static TypeInfo FromType(Type type)
        {
            return type == null ? null : new TypeInfo(type);
        }

        private TypeInfo(Type type)
        {
            Type = type;
        }

        public Type Type { get; }

        public bool IsGenericType => Type.IsGenericType;
        public bool IsClass => Type.IsClass;
        public bool IsAbstract => Type.IsAbstract;
        public bool IsGenericTypeDefinition => Type.IsGenericTypeDefinition;
        public bool IsArray => Type.IsArray;
        public string FullName => Type.FullName;
        public string Name => Type.Name;
        public string Namespace => Type.Namespace;
        public ITypeInfo BaseType => FromType(Type.BaseType);
        public bool IsGenericParameter => Type.IsGenericParameter;
        public bool IsEnum => Type.IsEnum;
        public ITypeInfo[] GenericTypeArguments => Type.GenericTypeArguments.Select(FromType).Cast<ITypeInfo>().ToArray();

        public ITypeInfo GetGenericTypeDefinition()
        {
            return FromType(Type.GetGenericTypeDefinition());
        }

        public ITypeInfo[] GetGenericArguments()
        {
            return Type.GetGenericArguments().Select(FromType).Cast<ITypeInfo>().ToArray();
        }

        public ITypeInfo[] GetInterfaces()
        {
            return Type.GetInterfaces().Select(FromType).Cast<ITypeInfo>().ToArray();
        }

        public ITypeInfo GetElementType()
        {
            return FromType(Type.GetElementType());
        }

        public string[] GetEnumNames()
        {
            return Type.GetEnumNames();
        }

        public IPropertyInfo[] GetProperties(BindingFlags bindingFlags)
        {
            return Type.GetProperties(bindingFlags).Select(x => (IPropertyInfo)new PropertyInfo(x)).ToArray();
        }

        public ITypeInfo[] GetCustomAttributes()
        {
            return Type.GetCustomAttributes().Select(x => (ITypeInfo)FromType(x.GetType())).ToArray();
        }

        public bool Equals(ITypeInfo other)
        {
            return Equals(Name, other?.Name) && Equals(Namespace, other?.Namespace) && IsGenericTypeDefinition == other?.IsGenericTypeDefinition && GenericArgumentsEquals(GetGenericArguments(), other?.GetGenericArguments());
        }

        private static bool GenericArgumentsEquals(ITypeInfo[] thisArgs, ITypeInfo[] otherArgs)
        {
            if (thisArgs == null && otherArgs == null)
                return true;
            if (thisArgs == null || otherArgs == null || thisArgs.Length != otherArgs.Length)
                return false;
            for (var i = 0; i < thisArgs.Length; i++)
            {
                if (!thisArgs[i].Equals(otherArgs[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TypeInfo)obj);
        }

        public override int GetHashCode()
        {
            return (Name, Namespace, IsGenericTypeDefinition).GetHashCode();
        }

        public override string ToString()
        {
            return $"TypeInfo({Type?.Name})";
        }
    }
}