﻿using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using SkbKontur.TypeScript.ContractGenerator;
using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.Extensions;
using SkbKontur.TypeScript.ContractGenerator.Internals;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders.ApiController;

using TypeInfo = SkbKontur.TypeScript.ContractGenerator.Internals.TypeInfo;
using AnotherTypeInfo = SkbKontur.TypeScript.ContractGenerator.Internals.TypeInfo;

namespace AspNetCoreExample.Generator
{
    public class ApiControllerTypeBuildingContext : ApiControllerTypeBuildingContextBase
    {
        public ApiControllerTypeBuildingContext(TypeScriptUnit unit, ITypeInfo type)
            : base(unit, type)
        {
        }

        public static bool Accept(ITypeInfo type)
        {
            var test1 = SkbKontur.TypeScript.ContractGenerator.Roslyn.TypeInfoRewriter.Types[0];
            var test2 = SkbKontur.TypeScript.ContractGenerator.Roslyn.TypeInfoRewriter.Types[1];
            var test3 = SkbKontur.TypeScript.ContractGenerator.Roslyn.TypeInfoRewriter.Types[2];
            var test4 = SkbKontur.TypeScript.ContractGenerator.Roslyn.TypeInfoRewriter.Types[3];

            return SkbKontur.TypeScript.ContractGenerator.Roslyn.TypeInfoRewriter.Types[4].IsAssignableFrom(type);
        }

        protected override TypeLocation GetApiBase(ITypeInfo controllerType)
        {
            var e = GetBody(null, null).Equals(null);
            var apiBaseName = GetApiBaseName(controllerType);
            return new TypeLocation
                {
                    Name = apiBaseName,
                    Location = "apiBase",
                };
        }

        private string GetApiBaseName(ITypeInfo controllerType)
        {
            if (IsUserScopedApi(controllerType))
                return "UserApiBase";
            return "ApiBase";
        }

        protected override ITypeInfo ResolveReturnType(ITypeInfo typeInfo)
        {
            if (typeInfo.IsGenericType)
            {
                var genericTypeDefinition = typeInfo.GetGenericTypeDefinition();
                if (genericTypeDefinition.Equals(SkbKontur.TypeScript.ContractGenerator.Roslyn.TypeInfoRewriter.Types[5]) || genericTypeDefinition.Equals(SkbKontur.TypeScript.ContractGenerator.Roslyn.TypeInfoRewriter.Types[6]))
                    return ResolveReturnType(typeInfo.GetGenericArguments()[0]);
            }

            if (typeInfo.Equals(SkbKontur.TypeScript.ContractGenerator.Roslyn.TypeInfoRewriter.Types[7]))
                return SkbKontur.TypeScript.ContractGenerator.Roslyn.TypeInfoRewriter.Types[8];
            if (typeInfo.Equals(SkbKontur.TypeScript.ContractGenerator.Roslyn.TypeInfoRewriter.Types[9]))
                return SkbKontur.TypeScript.ContractGenerator.Roslyn.TypeInfoRewriter.Types[10];
            return typeInfo;
        }

        protected override BaseApiMethod ResolveBaseApiMethod(IMethodInfo methodInfo)
        {
            if (methodInfo.GetAttributes(SkbKontur.TypeScript.ContractGenerator.Roslyn.TypeInfoRewriter.Types[11]).Any())
                return BaseApiMethod.Get;

            if (methodInfo.GetAttributes(SkbKontur.TypeScript.ContractGenerator.Roslyn.TypeInfoRewriter.Types[12]).Any())
                return BaseApiMethod.Post;

            if (methodInfo.GetAttributes(SkbKontur.TypeScript.ContractGenerator.Roslyn.TypeInfoRewriter.Types[13]).Any())
                return BaseApiMethod.Put;

            if (methodInfo.GetAttributes(SkbKontur.TypeScript.ContractGenerator.Roslyn.TypeInfoRewriter.Types[14]).Any())
                return BaseApiMethod.Delete;

            throw new NotSupportedException("Unresolved http verb for method  at controller ");
        }

        protected override string BuildRoute(ITypeInfo controllerType, IMethodInfo methodInfo)
        {
            var routeTemplate = methodInfo.GetAttributes(false)
                                          .Select(x => x.AttributeData.TryGetValue("Template", out var value) ? (string)value : null)
                                          .SingleOrDefault(x => !string.IsNullOrEmpty(x));
            return AppendRoutePrefix(routeTemplate, controllerType);
        }

        protected override bool PassParameterToCall(IParameterInfo parameterInfo, ITypeInfo controllerType)
        {
            if (IsUserScopedApi(controllerType) && parameterInfo.Name == "userId")
                return false;
            return true;
        }

        protected override IParameterInfo[] GetQueryParameters(IParameterInfo[] parameters, ITypeInfo controllerType)
        {
            return parameters.Where(x => PassParameterToCall(x, controllerType) && !x.GetAttributes(SkbKontur.TypeScript.ContractGenerator.Roslyn.TypeInfoRewriter.Types[15]).Any()).ToArray();
        }

        protected override IParameterInfo GetBody(IParameterInfo[] parameters, ITypeInfo controllerType)
        {
            return parameters.SingleOrDefault(x => PassParameterToCall(x, controllerType) && x.GetAttributes(SkbKontur.TypeScript.ContractGenerator.Roslyn.TypeInfoRewriter.Types[16]).Any());
        }

        protected override IMethodInfo[] GetMethodsToImplement(ITypeInfo controllerType)
        {
            return controllerType.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                                 .Where(m => !((MethodWrapper)m).Method.IsSpecialName)
                                 .Where(x => x.DeclaringType.Equals(controllerType))
                                 .ToArray();
        }

        private string AppendRoutePrefix(string routeTemplate, ITypeInfo controllerType)
        {
            var routeAttribute = controllerType.GetAttributes(SkbKontur.TypeScript.ContractGenerator.Roslyn.TypeInfoRewriter.Types[17]).SingleOrDefault();
            var fullRoute = (routeAttribute == null ? "" : routeAttribute.AttributeData["Template"] + "/") + routeTemplate;
            if (IsUserScopedApi(controllerType))
                return fullRoute.Substring("v1/user/{userId}/".Length);
            return fullRoute.Substring("v1/".Length);
        }

        private bool IsUserScopedApi(ITypeInfo controller)
        {
            var route = controller.GetAttributes(SkbKontur.TypeScript.ContractGenerator.Roslyn.TypeInfoRewriter.Types[18]).SingleOrDefault();
            var template = (string)route?.AttributeData["Template"];

            return template?.StartsWith("v1/user/{userId}") ?? false;
        }
    }
}