using System;
using System.Linq;
using System.Reflection;

namespace SwissAcademic.Addons.MacroManagerAddon
{
    internal static class ReflectionExt
    {
        internal static FieldInfo GetField<T>(this T obj, string name)
        {
            return
                obj
                .GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .FirstOrDefault(f => f.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        internal static object GetFieldValue<T>(this T obj, string name) => obj.GetField(name)?.GetValue(obj);

        internal static MethodInfo GetNonPublicMethodInfo<T>(this T obj, string name)
        {
            return
                 obj
                 .GetType()
                 .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                 .FirstOrDefault(mth => mth.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
