using Jil;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;

namespace KK.JilTest.Formatters
{
    static class TypedDeserializers
    {
        private static readonly ConcurrentDictionary<Type, Func<TextReader, Options, object>> methods;
        private static readonly MethodInfo method = typeof(JSON).GetMethod("Deserialize", new[] { typeof(TextReader), typeof(Options) });

        static TypedDeserializers()
        {
            methods = new ConcurrentDictionary<Type, Func<TextReader, Options, object>>();
        }

        public static Func<TextReader, Options, object> GetTyped(Type type)
        {
            return methods.GetOrAdd(type, CreateDelegate);
        }

        private static Func<TextReader, Options, object> CreateDelegate(Type type)
        {
            return (Func<TextReader, Options, object>)method
                .MakeGenericMethod(type)
                .CreateDelegate(typeof(Func<TextReader, Options, object>));
        }
    }
}