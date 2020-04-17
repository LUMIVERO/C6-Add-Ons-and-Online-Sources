using SwissAcademic.ApplicationInsights;
using SwissAcademic.Citavi;
using System;

namespace SwissAcademic.Addons.SortReferencesByParentChild
{
    internal static class ProjectExt
    {
        public static void AddComparerStatus(this Project project)
        {
            try
            {
                project.ProjectSettings[SortReferencesByParentChildAddOn.Key_Settings_Addon] = "true";
                project.ProjectSettings.Save(true);

            }
            catch (Exception ignored)
            {
                Telemetry.Error(ignored, string.Empty, ignored.Message, null);
            }
        }

        public static void RemoveComparerStatus(this Project project)
        {
            try
            {
                project.ProjectSettings[SortReferencesByParentChildAddOn.Key_Settings_Addon] = "false";
                project.ProjectSettings.Save(true);
            }
            catch (Exception ignored)
            {
                Telemetry.Error(ignored, string.Empty, ignored.Message, null);
            }
        }

        public static bool RestoreComparer(this Project project)
        {
            try
            {
                return bool.Parse(project.ProjectSettings[SortReferencesByParentChildAddOn.Key_Settings_Addon].ToString());
            }
            catch (Exception ignored)
            {
                Telemetry.Error(ignored, string.Empty, ignored.Message, null);
            }
            return false;
        }
    }
}
