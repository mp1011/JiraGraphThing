using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JiraGraphThing.Core.Extensions
{
    public static class ReflectionExtensions
    {
        public static MethodInfo GetMethodWithTypeArguments(this Type type, string methodName, Type[] typeArguments)
        {
            var methodWithName = type
                .GetMethods()
                .Where(m => m.Name == methodName)
                .ToArray();

        //making the assumption that there will only be one method with this number of generic arguments
        var method = methodWithName.Single(p => p.GetGenericArguments().Length == typeArguments.Length);
            return method.MakeGenericMethod(typeArguments);
        }

        public static object InvokeGenericMethod(this object o, string methodName, Type typeArgument, params object[] args)
        {
            return InvokeGenericMethod<object>(o, methodName, new Type[] { typeArgument }, args);
        }

        public static TResult InvokeGenericMethod<TResult>(this object o, string methodName, Type[] typeArguments, object[] args = null)
        {
            var result = o.GetType()
                .GetMethodWithTypeArguments(methodName, typeArguments)
                .Invoke(o, args);

            return (TResult)result;
        }

        public static void InvokeGenericMethod(this object o, string methodName, Type[] typeArguments, object[] args = null)
        {
            o.GetType()
                .GetMethodWithTypeArguments(methodName, typeArguments)
                .Invoke(o, args);
        }
    }
}
