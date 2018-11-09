﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using dexih.functions.Exceptions;
using Dexih.Utils.DataType;

namespace dexih.functions
{
    public class Functions
    {
        public static (string path, string pattern)[] SearchPaths()
        {
            return new[]
            {
                (Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "dexih.functions*.dll"),
                (Path.Combine(Directory.GetCurrentDirectory(), "plugins", "functions"), "*.dll")
            };
        }
        
        public static Type GetFunctionType(string className, string assemblyName = null)
        {
            Type type = null;

            if (string.IsNullOrEmpty(assemblyName))
            {
                type = Assembly.GetExecutingAssembly().GetType(className);
            }
            else
            {
                foreach (var path in SearchPaths())
                {
                    if (Directory.Exists(path.path))
                    {
                        var fileName = Path.Combine(path.path, assemblyName);
                        if (System.IO.File.Exists(fileName))
                        {
                            var assembly = Assembly.LoadFile(fileName);
                            type = assembly.GetType(className);
                            break;
                        }
                    }
                }
            }

            if (type == null)
            {
                throw new FunctionNotFoundException($"The type {className} was not found.");
            }

            return type;
        }
        
        public static (Type type, MethodInfo method) GetFunctionMethod(string className, string methodName, string assemblyName = null)
        {
            var type = GetFunctionType(className, assemblyName);

            var method = type.GetMethod(methodName);

            if (method == null)
            {
                throw new FunctionNotFoundException($"The method {methodName} was not found in the type {className}.");
            }

            return (type, method);
        }
        
        public static FunctionReference GetFunction(string className, string methodName, string assemblyName = null)
        {
            var functionMethod = GetFunctionMethod(className, methodName, assemblyName);

            if (functionMethod.method == null)
            {
                throw new FunctionNotFoundException($"The method {methodName} was not found in the type {className}.");
            }

            var function = GetFunction(functionMethod.type, functionMethod.method);
            function.FunctionAssemblyName = assemblyName;

            return function;
        }

        private static DataType.ETypeCode? GetElementType(Type p, out int rank)
        {
            if (p == typeof(void))
            {
                rank = 0;
                return null;
            }
            
            return DataType.GetTypeCode(p, out rank);   
        }
//
//        private static int GetRank(Type p)
//        {
//            if (typeof(void) == p) return 0;
//            return p.IsArray ? p.GetArrayRank() : 0;
//        }

        public static FunctionReference GetFunction(Type type, MethodInfo method)
        {
            var attribute = method.GetCustomAttribute<TransformFunctionAttribute>();
            if (attribute != null)
            {
                MethodInfo resultMethod = null;
                if (attribute.ResultMethod != null)
                {
                    resultMethod = type.GetMethod(attribute.ResultMethod);
                }

                var compareAttribute = method.GetCustomAttribute<TransformFunctionCompareAttribute>();

                var function = new FunctionReference()
                {
                    Name = attribute.Name,
                    Description = attribute.Description,
                    FunctionType = attribute.FunctionType,
                    Category = attribute.Category,
                    FunctionMethodName = method.Name,
                    ResetMethodName = attribute.ResetMethod,
                    ResultMethodName = attribute.ResultMethod,
                    ImportMethodName = attribute.ImportMethod,
                    IsStandardFunction = true,
                    Compare = compareAttribute?.Compare,
                    FunctionClassName = type.FullName,
                    GenericType = attribute.GenericType,
                    GenericTypeDefault = attribute.GenericTypeDefault,
                    ReturnParameter = GetFunctionParameter(method.ReturnParameter, "Return"),
                    InputParameters = method.GetParameters().Where(c =>
                    {
                        var variable = c.GetCustomAttribute<TransformFunctionVariableAttribute>();
                        return !c.IsOut && variable == null;
                    }).Select(p => GetFunctionParameter(p)).ToArray(),
                    OutputParameters = method.GetParameters().Where(c => c.IsOut).Select(p => GetFunctionParameter(p)).ToArray(),
                    ResultReturnParameter = GetFunctionParameter(resultMethod?.ReturnParameter, "Group Return"),
                    ResultInputParameters = resultMethod?.GetParameters().Where(c =>
                    {
                        var variable = c.GetCustomAttribute<TransformFunctionVariableAttribute>();
                        return !c.IsOut && variable == null;
                    }).Select(p => GetFunctionParameter(p)).ToArray(),
                    ResultOutputParameters = resultMethod?.GetParameters().Where(c => c.IsOut).Select(p => GetFunctionParameter(p)).ToArray()
                };

                return function;
            }

            return null;
        }

        private static FunctionParameter GetFunctionParameter(ParameterInfo parameterInfo, string name = null)
        {
            if (parameterInfo == null) return null;

            var parameterAttribute = parameterInfo.GetCustomAttribute<TransformFunctionParameterAttribute>();

            Type paramType;
            if (parameterInfo.ParameterType.IsByRef)
            {
                paramType = parameterInfo.ParameterType.GetElementType();
            }
            else
            {
                paramType = parameterInfo.ParameterType;
            }
            
            var isGeneric = 
                (paramType.IsArray && paramType.GetElementType().IsGenericParameter) || 
                (paramType.IsGenericParameter);

            return new FunctionParameter()
            {
                ParameterName = name ?? parameterInfo.Name,
                Name = name ?? parameterAttribute?.Name ?? parameterInfo.Name,
                Description = parameterAttribute?.Description,
                IsGeneric = isGeneric,
                DataType = DataType.GetTypeCode(paramType, out var paramRank),
                AllowNull = Nullable.GetUnderlyingType(paramType) != null,
                Rank = paramRank,
                IsTwin = parameterInfo.GetCustomAttribute<TransformFunctionParameterTwinAttribute>() != null,
                ListOfValues = EnumValues(parameterInfo),
                DefaultValue = DefaultValue(parameterInfo)
            };
        }

        // convert enum to list of values
        private static string[] EnumValues(ParameterInfo p)
        {
            if (p.ParameterType.IsEnum)
            {
                return Enum.GetNames(p.ParameterType);
            }
            else
            {
                return null;
            }
        }

        private static object DefaultValue(ParameterInfo p)
        {
            if (p.ParameterType.IsEnum)
            {
                return p.DefaultValue?.ToString();
            }
            else
            {
                return p.DefaultValue;
            }
        }

        public static List<FunctionReference> GetAllFunctions()
        {
            var functions = new List<FunctionReference>();

            foreach (var path in SearchPaths())
            {
                if (Directory.Exists(path.path))
                {
                    foreach (var file in Directory.GetFiles(path.path, path.pattern))
                    {
                        var assembly = Assembly.LoadFrom(file);
                        var types = assembly.GetTypes();
                        // var types = AppDomain.CurrentDomain.GetAssemblies().Where(c=>c.FullName.StartsWith("dexih.functions")).SelectMany(s => s.GetTypes());

                        var assemblyName = Path.GetFileName(file);
                        if (assemblyName == Path.GetFileName(Assembly.GetExecutingAssembly().Location))
                        {
                            assemblyName = null;
                        }
                        foreach (var type in types)
                        {
                            foreach (var method in type.GetMethods())
                            {
                                var function = GetFunction(type, method);
                                if (function != null)
                                {
                                    function.FunctionAssemblyName = assemblyName;
                                    functions.Add(function);
                                }
                            }
                        }
                    }
                }
            }

            return functions;
        }
        
//        public static List<FunctionReference> GetAllFunctions()
//        {
//            var types = AppDomain.CurrentDomain.GetAssemblies().Where(c=>c.FullName.StartsWith("dexih.functions"))
//                .SelectMany(s => s.GetTypes());
//
//            var methods = new List<FunctionReference>();
//            
//            foreach (var type in types)
//            {
//                foreach (var method in type.GetMethods())
//                {
//                    var function = GetFunction(type, method);
//                    if(function != null)
//                    {
//                        methods.Add(function);
//                    }
//                }
//            }
//            return methods;
//        }
    }
}