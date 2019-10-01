using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditor
{
    public static class ReflectionHelper
    {
        public static T CreateInstanceOf<T>(params object[] args)
        {
            var type = typeof(T);
            var instance = type.Assembly.CreateInstance(
                type.FullName, false,
                BindingFlags.Instance | BindingFlags.NonPublic,
                null, args, null, null);
            return (T)instance;
        }

        public static B Property<T, B>(this T t, string propertyName) where T : class
        {
            var o = t
                   .Members(propertyName, MemberTypes.Property, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                   .Cast<PropertyInfo>()
                   .FirstOrDefault(prop => prop.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase))?
                   .GetValue(t);
            return (B)Convert.ChangeType(o, typeof(B));
        }

        public static B Field<T, B>(this T t, string fieldName) where T : class
        {
            var o = t
                   .Members(fieldName, MemberTypes.Field, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                   .Cast<FieldInfo>()
                   .FirstOrDefault(field => field.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase))?
                   .GetValue(t);
            return (B)Convert.ChangeType(o, typeof(B));
        }

        public static void Property<T, B>(this T t, string propertyName, B value) where T : class
        {
            t
            .Members(propertyName, MemberTypes.Property, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .Cast<PropertyInfo>()
            .FirstOrDefault(prop => prop.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase))?
            .SetValue(t, value);
        }

        public static void Field<T, B>(this T t, string fieldName, B value) where T : class
        {
            t
            .Members(fieldName, MemberTypes.Field, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .Cast<FieldInfo>()
            .FirstOrDefault(field => field.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase))?
            .SetValue(t, value);
        }

        public static object Invoke<T>(this T t, string methodName, params object[] parameters) where T : class
        {
            var memberInfos = t.Members(methodName, MemberTypes.Method, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            return memberInfos.Cast<MethodInfo>().FirstOrDefault(method => method.GetParameters().AreEqual(parameters))?.Invoke(t, parameters);
        }

        public static Task InvokeAsync<T>(this T t, string methodName, params object[] parameters) where T : class
        {
            return (Task)t.Invoke(methodName, parameters);
        }

        private static IEnumerable<MemberInfo> Members<T>(this T t, string memberName, MemberTypes memberType, BindingFlags bindingFlags)
        {
            return t
                   .GetType()
                   .GetMembers(bindingFlags)
                   .Where(prop => prop.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase) && prop.MemberType == memberType)
                   .ToList();
        }

        private static bool AreEqual(this ParameterInfo[] parameterInfos, object[] parameters)
        {
            if (parameterInfos?.Length != parameters?.Length) return false;
            for (int i = 0; i < parameterInfos.Length; i++)
            {
                if (parameterInfos[i].ParameterType != (parameters[i]).GetType()) return false;
            }
            return true;
        }
    }
}

