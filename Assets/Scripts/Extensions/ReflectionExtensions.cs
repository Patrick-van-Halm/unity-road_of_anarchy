using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class ReflectionExtensions
{
    public static object Invoke(this MethodInfo method, object obj)
    {
        return method.Invoke(obj, new object[] {});
    }
}
