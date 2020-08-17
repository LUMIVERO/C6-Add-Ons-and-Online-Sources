using SwissAcademic.Citavi.Shell;
using SwissAcademic.Citavi.Shell.Controls.Editors;
using System;
using System.Reflection;

namespace SwissAcademic.Addons.MacroManagerAddon
{
    internal static class MacroEditorFormExt
    {
        public static void Run(this MacroEditorForm macroEditorForm)
        {
            macroEditorForm
                .GetNonPublicMethodInfo("PerformCommand")?
                .Invoke(macroEditorForm, new object[] { "Run", null, null, null });
        }

        public static void SetFilePath(this MacroEditorForm macroEditorForm, string filePath)
        {
            macroEditorForm
                .GetField("_fileName")?
                .SetValue(macroEditorForm, filePath);
        }

        public static bool IsDirty(this MacroEditorForm macroEditorForm)
        {
            if (macroEditorForm.GetFieldValue("macroEditor") is MacroEditorControl macroEditorControl)
            {
                return macroEditorControl.Dirty;
            }
            return false;
        }

        public static void Save(this MacroEditorForm macroEditorForm)
        {
            macroEditorForm
                .GetNonPublicMethodInfo("PerformCommand")?
                .Invoke(macroEditorForm, new object[] { "Save", null, null, null });
        }

        public static void SetAsDefault(this MacroEditorForm macroEditorForm)
        {
            try
            {
                typeof(Program)
                    .GetField("_macroEditor", BindingFlags.Static | BindingFlags.NonPublic)?
                    .SetValue(null, macroEditorForm);
            }
            catch (Exception)
            {
            }
        }
    }
}
