using System;
using System.Collections.Generic;
using System.Text;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Internals;
using SkbKontur.TypeScript.ContractGenerator.Types;

namespace SkbKontur.TypeScript.ContractGenerator
{
    public class TypeScriptUnit
    {
        public string Path { get; set; }

        public IEnumerable<TypeScriptImportStatement> Imports => imports.Values;

        public List<TypeScriptStatement> Body { get; } = new List<TypeScriptStatement>();

        public TypeScriptTypeReference AddTypeImport(ITypeInfo sourceType, TypeScriptTypeDeclaration typeDeclaration, TypeScriptUnit sourceUnit)
        {
            if (sourceUnit != this && !imports.ContainsKey(sourceType))
            {
                imports.Add(sourceType, new TypeScriptImportFromUnitStatement
                    {
                        TypeName = typeDeclaration.Name,
                        CurrentUnit = this,
                        TargetUnit = sourceUnit,
                    });
            }
            return new TypeScriptTypeReference(typeDeclaration.Name);
        }

        public TypeScriptVariableReference AddSymbolImport(string symbolName, string path)
        {
            var importedSymbol = new ImportedSymbol(symbolName, symbolName, path);
            if (!symbolImports.ContainsKey(importedSymbol))
            {
                symbolImports.Add(importedSymbol, new TypeScriptImportFromPathStatement
                    {
                        TypeName = symbolName,
                        CurrentUnit = this,
                        PathToUnit = path,
                    });
            }
            return new TypeScriptVariableReference(symbolName);
        }

        public TypeScriptVariableReference AddDefaultSymbolImport(string localName, string path)
        {
            var importedSymbol = new ImportedSymbol("default", localName, path);
            if (!symbolImports.ContainsKey(importedSymbol))
            {
                symbolImports.Add(importedSymbol, new TypeScriptImportDefaultFromPathStatement
                    {
                        TypeName = localName,
                        CurrentUnit = this,
                        PathToUnit = path,
                    });
            }
            return new TypeScriptVariableReference(localName);
        }

        public string GenerateCode(DefaultCodeGenerationContext context)
        {
            var result = new StringBuilder();

            foreach (var import in Imports)
            {
                result.Append(import.GenerateCode(context)).Append(context.NewLine);
            }
            foreach (var import in symbolImports.Values)
            {
                result.Append(import.GenerateCode(context)).Append(context.NewLine);
            }
            result.Append(context.NewLine);

            foreach (var statement in Body)
            {
                result.Append(statement.GenerateCode(context)).Append(context.NewLine);
            }

            return result.ToString();
        }

        private readonly Dictionary<ITypeInfo, TypeScriptImportStatement> imports = new Dictionary<ITypeInfo, TypeScriptImportStatement>();
        private readonly Dictionary<ImportedSymbol, TypeScriptImportStatement> symbolImports = new Dictionary<ImportedSymbol, TypeScriptImportStatement>();
    }
}