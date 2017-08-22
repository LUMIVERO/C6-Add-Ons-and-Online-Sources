// ######################################
// #                                    #
// #    Copyright                       #
// #    Daniel Lutz                     #
// #    Swiss Academic Software GmbH    #
// #    2014                            #
// #                                    #
// ######################################

namespace ManageMacrosAddon
{
    public static class AddonInfoHelper
    {
        #region ValidConfiguration

        public static bool ValidConfiguration(AddonInfo addonInfo)
        {
            if (string.IsNullOrEmpty(addonInfo.Configuration)) return false;

            if (!System.IO.File.Exists(addonInfo.Configuration)) return false;

            var content = System.IO.File.ReadAllText(addonInfo.Configuration);

            if (string.IsNullOrEmpty(content)) return false;

            return System.IO.Directory.Exists(content);
        }

        #endregion

        #region GetConfigurationContentAsFilePath

        public static string GetConfigurationContentAsFilePath(AddonInfo addonInfo)
        {
            return System.IO.File.ReadAllText(addonInfo.Configuration);
        }

        #endregion
    }
}
