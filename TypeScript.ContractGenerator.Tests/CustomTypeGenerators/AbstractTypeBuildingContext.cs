using System;
using System.Linq;
using System.Reflection;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;
using SkbKontur.TypeScript.ContractGenerator.Types;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.CustomTypeGenerators
{
    public class AbstractTypeBuildingContext : TypeBuildingContext
    {
        public AbstractTypeBuildingContext(TypeScriptUnit unit, ITypeInfo type)
            : base(unit, type)
        {
            this.unit = unit;
            this.type = type;
        }

        public override void Initialize(ITypeGenerator typeGenerator)
        {
            var types = typeGenerator.TypesProvider
                                     .GetTypes()
                                     .Where(x => x.BaseType.Equals(type))
                                     .ToArray();

            Declaration = new TypeScriptTypeDeclaration
                {
                    Name = type.Name,
                    Definition = new TypeScriptUnionType(types.Select(x =>
                        {
                            var resultType = typeGenerator.BuildAndImportType(unit, x, x);
                            if(resultType is INullabilityWrapperType nullableType)
                            {
                                return nullableType.InnerType;
                            }
                            return resultType;
                        }).ToArray())
                };
            base.Initialize(typeGenerator);
        }

        private readonly TypeScriptUnit unit;
        private readonly ITypeInfo type;
    }
}