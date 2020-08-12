using SwissAcademic.ApplicationInsights;
using SwissAcademic.Citavi;
using System;

namespace SwissAcademic.Addons.SortReferencesByParentChildAddon
{
    internal static class ProjectExt
    {
        public static void AddComparerStatus(this Project project)
        {
            try
            {
                project.ProjectSettings[Addon.SettingsKey] = "true";
                project.ProjectSettings.Save(true);

            }
            catch (Exception ignored)
            {
                Telemetry.Warning(ignored, string.Empty, ignored.Message, null);
            }
        }

        public static void RemoveComparerStatus(this Project project)
        {
            try
            {
                project.ProjectSettings[Addon.SettingsKey] = "false";
                project.ProjectSettings.Save(true);
            }
            catch (Exception ignored)
            {
                Telemetry.Warning(ignored, string.Empty, ignored.Message, null);
            }
        }

        public static bool RestoreComparer(this Project project)
        {
            try
            {
                return bool.Parse(project.ProjectSettings[Addon.SettingsKey].ToString());
            }
            catch (Exception ignored)
            {
                Telemetry.Warning(ignored, string.Empty, ignored.Message, null);
            }
            return false;
        }
    }
}
