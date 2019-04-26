using System.Reflection;

namespace SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditor
{
    public static class Reflection2
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
    }
}
