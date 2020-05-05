using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditorAddon
{
    public static class ReflectionHelper
    {
        public static TResult Field<TObject, TResult>(this TObject t, string fieldName) where TObject : class
        {
            var o = t
                   .Members(fieldName, MemberTypes.Field, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                   .Cast<FieldInfo>()
                   .FirstOrDefault(field => field.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase))?
                   .GetValue(t);
            return (TResult)Convert.ChangeType(o, typeof(TResult));
        }

        public static object Invoke<TObject>(this TObject t, string methodName, params object[] parameters) where TObject : class
        {
            var memberInfos = t.Members(methodName, MemberTypes.Method, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            return memberInfos.Cast<MethodInfo>().FirstOrDefault(method => method.GetParameters().AreEqual(parameters))?.Invoke(t, parameters);
        }

        private static IEnumerable<MemberInfo> Members<TObject>(this TObject t, string memberName, MemberTypes memberType, BindingFlags bindingFlags)
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

