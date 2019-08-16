using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Types;

namespace SkbKontur.TypeScript.ContractGenerator
{
    public interface IPropertyResolver
    {
        TypeScriptTypeMemberDeclaration ResolveProperty(TypeScriptUnit unit, ITypeGenerator typeGenerator, ITypeInfo type, IPropertyInfo property);
    }
}