using System;
using System.Linq;

using JetBrains.Annotations;

namespace SkbKontur.TypeScript.ContractGenerator.Types
{
    public class RootTypesProvider : IRootTypesProvider
    {
        public RootTypesProvider(params Type[] rootTypes)
        {
            this.rootTypes = rootTypes ?? new Type[0];
        }

        [NotNull, ItemNotNull]
        public ITypeInfo[] GetRootTypes()
        {
            return rootTypes.Select(TypeInfo.FromType).Cast<ITypeInfo>().ToArray();
        }

        public ITypeInfo GetType(string name)
        {
            return TypeInfo.FromType(rootTypes[0].Assembly.GetType(name));
        }

        public ITypeInfo[] GetTypes()
        {
            return rootTypes.SelectMany(x => x.Assembly.GetTypes()).Select(TypeInfo.FromType).Cast<ITypeInfo>().ToArray();
        }

        [NotNull]
        public static IRootTypesProvider Default = new RootTypesProvider();

        private readonly Type[] rootTypes;
    }
}