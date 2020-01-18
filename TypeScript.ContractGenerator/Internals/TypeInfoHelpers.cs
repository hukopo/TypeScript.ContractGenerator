﻿using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.Internals
{
    public static class TypeInfoHelpers
    {
        public static bool Equals(ITypeInfo self, ITypeInfo other)
        {
            if (self == null && other == null)
                return true;

            if (self == null || other == null)
                return false;

            if (self.IsArray)
                return other.IsArray && self.GetElementType().Equals(other.GetElementType());

            if (self.IsGenericParameter)
                return other.IsGenericParameter && self.Name == other.Name;

            return self.Name == other.Name
                   && self.Namespace == other.Namespace
                   && self.IsGenericTypeDefinition == other.IsGenericTypeDefinition
                   && GenericArgumentsEquals(self.GetGenericArguments(), other.GetGenericArguments());
        }

        public static int GetHashCode(ITypeInfo self)
        {
            if (self.IsArray)
                return (self.IsArray, self.GetElementType()).GetHashCode();

            if (self.IsGenericParameter)
                return (self.IsGenericParameter, self.Name).GetHashCode();

            return (self.Name, self.Namespace, self.IsGenericTypeDefinition).GetHashCode();
        }

        private static bool GenericArgumentsEquals(ITypeInfo[] thisArgs, ITypeInfo[] otherArgs)
        {
            if (thisArgs.Length != otherArgs.Length)
                return false;
            for (var i = 0; i < thisArgs.Length; i++)
                if (!thisArgs[i].Equals(otherArgs[i]))
                    return false;
            return true;
        }
    }
}