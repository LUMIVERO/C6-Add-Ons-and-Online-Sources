using Newtonsoft.Json;
using SwissAcademic.Citavi.Shell;
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

    public static class Extensions
    {
        public static AddonSettings Load(this string settings)
        {
            if (string.IsNullOrEmpty(settings)) return AddonSettings.Default;
            return JsonConvert.DeserializeObject<AddonSettings>(settings);
        }

        public static string ToJson(this AddonSettings settings)
        {
            if (settings == null) return string.Empty;

            return JsonConvert.SerializeObject(settings);
        }


        public static WorkSpace CreateWorkSpaceByName(this ReferenceGridForm referenceGridForm, string name)
        {
            return new WorkSpace();
        }

    }
}
