using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Mercure.API.Tests.Models;

/// <summary>
/// Assembly loader
/// </summary>
public class AssemblyLoader
{
    private readonly Assembly _assembly;

    /// <summary>
    /// Constructor
    /// </summary>
    public AssemblyLoader()
    {
        _assembly = Assembly.Load("Mercure.API");
    }

    /// <summary>
    /// Get type by name
    /// </summary>
    /// <param name="typeName"></param>
    /// <returns></returns>
    public Type GetType(string typeName)
    {
        return
            _assembly
                .GetTypes()
                .FirstOrDefault(t => t.Name.ToLower().Contains(typeName));
    }

    /// <summary>
    /// Get constructor by types
    /// </summary>
    /// <param name="typeName"></param>
    /// <param name="types"></param>
    /// <returns></returns>
    public ConstructorInfo GetConstructorByTypes(string typeName, List<Type> types)
    {
        return
            GetType(typeName)
                ?.GetConstructor(types.ToArray());
    }

    /// <summary>
    /// Get property by name
    /// </summary>
    /// <param name="typeName"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public PropertyInfo GetProperty(string typeName, string propertyName)
    {
        return
            GetType(typeName)
                .GetProperties()
                .FirstOrDefault(p => p.Name.ToLower() == propertyName);
    }

    /// <summary>
    /// Get property by name
    /// </summary>
    /// <param name="type"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public PropertyInfo GetProperty(Type type, string propertyName)
    {
        return
            type
                .GetProperties()
                .FirstOrDefault(p => p.Name.ToLower() == propertyName);
    }

    /// <summary>
    /// Get property type by name
    /// </summary>
    /// <param name="typeName"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public Type GetPropertyType(string typeName, string propertyName)
    {
        return
            GetType(typeName)
                .GetProperties()
                .FirstOrDefault(p => p.Name.ToLower() == propertyName)
                ?.PropertyType;
    }

    /// <summary>
    /// Get property value by name
    /// </summary>
    /// <param name="typeName"></param>
    /// <param name="propertyName"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public object GetPropertyValue(string typeName, string propertyName, object obj)
    {
        return
            GetType(typeName)
                .GetProperties()
                .FirstOrDefault(p => p.Name.ToLower() == propertyName)
                ?.GetValue(obj);
    }

    /// <summary>
    /// Get property value by name
    /// </summary>
    /// <param name="typeName"></param>
    /// <param name="interfaceName"></param>
    /// <returns></returns>
    public object GetImplementedInterface(string typeName, string interfaceName)
    {
        return
            GetType(typeName)
                .GetInterfaces()
                .FirstOrDefault(i => i.Name.ToLower() == interfaceName);
    }

    /// <summary>
    /// Get method by name
    /// </summary>
    /// <param name="typeName"></param>
    /// <param name="methodName"></param>
    /// <returns></returns>
    public MethodInfo GetMethod(string typeName, string methodName)
    {
        return
            GetType(typeName)
                .GetMethods()
                .FirstOrDefault(i => i.Name.ToLower() == methodName);
    }

    /// <summary>
    /// Get method parameters types
    /// </summary>
    /// <param name="typeName"></param>
    /// <param name="methodName"></param>
    /// <returns></returns>
    public List<Type> GetMethodParametersTypes(string typeName, string methodName)
    {
        return
            GetType(typeName)
                .GetMethods()
                .Where(m => m.Name.ToLower() == methodName)
                .FirstOrDefault()
                ?.GetParameters()
                .Select(p => p.ParameterType)
                .ToList();
    }

    /// <summary>
    /// Get method parameters
    /// </summary>
    /// <param name="typeName"></param>
    /// <param name="methodName"></param>
    /// <returns></returns>
    public List<ParameterInfo> GetMethodParameters(string typeName, string methodName)
    {
        return
            GetMethod(typeName, methodName)
                .GetParameters()
                .ToList();
    }

    /// <summary>
    /// Get runtime method by name
    /// </summary>
    /// <param name="typeName"></param>
    /// <param name="methodName"></param>
    /// <returns></returns>
    public MethodInfo GetRuntimeMethod(string typeName, string methodName)
    {
        return
            GetType(typeName)
                .GetRuntimeMethods()
                .FirstOrDefault(i => i.Name.ToLower() == methodName);
    }

    /// <summary>
    /// Get runtime method by name
    /// </summary>
    /// <param name="type"></param>
    /// <param name="methodName"></param>
    /// <returns></returns>
    public MethodInfo GetRuntimeMethod(Type type, string methodName)
    {
        return
            type
                .GetRuntimeMethods()
                .FirstOrDefault(i => i.Name.ToLower() == methodName);
    }

    /// <summary>
    /// Get runtime method by name
    /// </summary>
    /// <param name="typeName"></param>
    /// <param name="methodName"></param>
    /// <param name="types"></param>
    /// <returns></returns>
    public MethodInfo GetRuntimeMethodByParametersTypes(string typeName, string methodName, List<Type> types)
    {
        return
            GetType(typeName)
                .GetRuntimeMethod(methodName, types.ToArray());
    }

    /// <summary>
    /// Get runtime method parameters types
    /// </summary>
    /// <param name="typeName"></param>
    /// <param name="methodName"></param>
    /// <returns></returns>
    public List<ParameterInfo> GetRuntimeMethodParameters(string typeName, string methodName)
    {
        return
            GetRuntimeMethod(typeName, methodName)
                .GetParameters()
                .ToList();
    }

    /// <summary>
    /// Get runtime method parameters types
    /// </summary>
    /// <param name="typeName"></param>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    public FieldInfo GetPrivateRuntimeField(string typeName, string fieldName)
    {
        return
            GetType(typeName)
                ?.GetRuntimeFields()
                .FirstOrDefault(f => f.IsPrivate && f.Name.Contains(fieldName));
    }

    /// <summary>
    /// Get runtime method parameters types
    /// </summary>
    /// <param name="type"></param>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    public FieldInfo GetPrivateRuntimeField(Type type, string fieldName)
    {
        return
            type
                ?.GetRuntimeFields()
                .FirstOrDefault(f => f.IsPrivate && f.Name.Contains(fieldName));
    }

    /// <summary>
    /// Get runtime method parameters types
    /// </summary>
    /// <param name="typeName"></param>
    /// <param name="genericTypeName"></param>
    /// <returns></returns>
    public Type MakeGenericType(string typeName, string genericTypeName)
    {
        return GetType(typeName).MakeGenericType(GetType(genericTypeName));
    }

    /// <summary>
    /// Get runtime method parameters types
    /// </summary>
    /// <param name="typeName"></param>
    /// <param name="genericTypes"></param>
    /// <returns></returns>
    public Type MakeGenericTypes(string typeName, params Type[] genericTypes)
    {
        return GetType(typeName).MakeGenericType(genericTypes);
    }
}