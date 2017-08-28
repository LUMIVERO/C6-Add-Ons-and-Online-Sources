namespace SwissAcademic.Addons.MacroManager
{
    public static class AddonInfoHelper
    {
        public static bool ValidConfiguration(AddonInfo addonInfo)
        {
            if (string.IsNullOrEmpty(addonInfo.Configuration)) return false;

            if (!System.IO.File.Exists(addonInfo.Configuration)) return false;

            var content = System.IO.File.ReadAllText(addonInfo.Configuration);

            if (string.IsNullOrEmpty(content)) return false;

            return System.IO.Directory.Exists(content);
        }

        public static string GetConfigurationContentAsFilePath(AddonInfo addonInfo)
        {
            return System.IO.File.ReadAllText(addonInfo.Configuration);
        }

    }
}
