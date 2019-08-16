using JetBrains.Annotations;

namespace SkbKontur.TypeScript.ContractGenerator.Types
{
    public interface IRootTypesProvider
    {
        [NotNull]
        ITypeInfo[] GetRootTypes();

        ITypeInfo GetType(string name);

        ITypeInfo[] GetTypes();
    }
}