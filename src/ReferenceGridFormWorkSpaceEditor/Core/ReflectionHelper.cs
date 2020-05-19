using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditorAddon
{
    public static class ReflectionHelper
    {
        public static TResult Field<TObject, TResult>(this TObject tObject, string fieldName) where TObject : class
        {
            var o = tObject
                   .Members(fieldName, MemberTypes.Field, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                   .Cast<FieldInfo>()
                   .FirstOrDefault(field => field.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase))?
                   .GetValue(tObject);
            return (TResult)Convert.ChangeType(o, typeof(TResult));
        }

        public static object Invoke<TObject>(this TObject tObject, string methodName, params object[] parameters) where TObject : class
        {
            return tObject
                   .Members(methodName, MemberTypes.Method, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                   .Cast<MethodInfo>()
                   .FirstOrDefault(method => method.GetParameters().AreEqual(parameters))
                   ?.Invoke(tObject, parameters);
        }

        private static IEnumerable<MemberInfo> Members<TObject>(this TObject tObject, string memberName, MemberTypes memberType, BindingFlags bindingFlags)
        {
            return tObject.GetType()
                          .GetMembers(bindingFlags)
                          .Where(prop => prop.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase) && prop.MemberType == memberType)
                          .ToList();
        }

        private static bool AreEqual(this ParameterInfo[] parameterInfos, object[] parameters)
        {
            if (parameterInfos?.Length != parameters?.Length)
            {
                return false;
            }

            for (int i = 0; i < parameterInfos.Length; i++)
            {
                if (parameterInfos[i].ParameterType != parameters[i].GetType())
                {
                    return false;
                }
            }
            return true;
        }
    }
}

