using Newtonsoft.Json;
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DonkeyLive.Shared.Extensions;

public static class TypeExtensions
{
    public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
    {
        var interfaceTypes = givenType.GetInterfaces();

        foreach (var it in interfaceTypes)
        {
            if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }
        }

        if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
        {
            return true;
        }

        var baseType = givenType.BaseType;
        if (baseType == null)
        {
            return false;
        }

        return baseType.IsAssignableToGenericType(genericType);
    }

    public static bool IsEmpty([NotNullWhen(false)] this object? value)
    {
        if (value is null)
        {
            return true;
        }

        if (value is string && string.IsNullOrWhiteSpace(value as string))
        {
            return true;
        }

        if (value is Guid guid)
        {
            return guid == Guid.Empty;
        }

        if (value is IList collection)
        {
            return collection.Count == 0;
        }

        if (value is DateTime dateTime)
        {
            return dateTime == DateTime.MinValue;
        }

        return false;
    }

    public static bool IsNotEmpty([NotNullWhen(true)] this object? value)
    {
        return !value.IsEmpty();
    }

    public static bool IsNull([NotNullWhen(false)] this object? value)
    {
        return value is null;
    }

    public static bool IsNotNull([NotNullWhen(true)] this object? value)
    {
        return !value.IsNull();
    }

    public static T As<T>(this object value)
    {
        Debug.Assert(value is T, "value is not of type T");
        Debug.Assert(value.IsNotNull(), "value is null");

        return (T)value;
    }

    public static T? AsOrDefault<T>(this object? value, T? defaultValue = default)
    {
        return value is T t ? t : defaultValue;
    }

    public static string ToJson(this object? obj)
    {
        try
        {
            if (obj.IsNull())
            {
                return string.Empty;
            }

            return JsonConvert.SerializeObject(obj);
        }
        catch
        {
            return string.Empty;
        }
    }

    public static T? ToObject<T>(this string? json, T? defaultValue = default)
    {
        try
        {
            if(json.IsEmpty())
            {
                return defaultValue;
            }

            return JsonConvert.DeserializeObject<T>(json);
        }
        catch
        {
            return defaultValue;
        }
    }
}
