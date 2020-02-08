using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiraGraphThing.Core.Extensions
{
    public static class SimpleIocExtensions
    {
        /// <summary>
        /// Registers all types implementing the given type, and registers an array of all such types
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ioc"></param>
        public static void RegisterAllAsArray<T>(this SimpleIoc ioc)
        {
            List<T> items = new List<T>();

            var assembly = typeof(T).Assembly;
            foreach (var type in assembly.GetTypes())
            {
                if (typeof(T).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
                {
                    ioc.Register(type);
                    items.Add((T)ioc.GetInstance(type));
                }
            }

            ioc.Register(() => items.ToArray());
        }

        public static void Register(this SimpleIoc ioc, Type t)
        {
            var method = ioc.GetType()
                .GetMethods()
                .Single(p => p.Name == nameof(ioc.Register)
                    && p.GetGenericArguments().Length == 1
                    && p.GetParameters().Length == 0);

            method.MakeGenericMethod(t).Invoke(ioc,null);
        }
    }
}
