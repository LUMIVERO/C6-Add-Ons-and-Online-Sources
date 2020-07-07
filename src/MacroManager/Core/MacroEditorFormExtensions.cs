using SwissAcademic.Citavi.Shell;
using SwissAcademic.Citavi.Shell.Controls.Editors;
using System;
using System.Linq;
using System.Reflection;

namespace SwissAcademic.Addons.MacroManagerAddon
{
    internal static class MacroEditorFormExtensions
    {
        public static void Run(this MacroEditorForm macroEditorForm)
        {
            macroEditorForm
                .GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                .FirstOrDefault(mth => mth.Name.Equals("PerformCommand", StringComparison.OrdinalIgnoreCase))?
                .Invoke(macroEditorForm, new object[] { "Run", null, null, null });
        }

        public static void SetFilePath(this MacroEditorForm macroEditorForm, string filePath)
        {
            macroEditorForm.GetType()
                           .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                           .FirstOrDefault(f => f.Name.Equals("_fileName", StringComparison.OrdinalIgnoreCase))?
                           .SetValue(macroEditorForm, filePath);
        }

        public static bool IsDirty(this MacroEditorForm macroEditorForm)
        {
            var fieldValue = macroEditorForm.GetType()
                                            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                                            .FirstOrDefault(f => f.Name.Equals("macroEditor", StringComparison.OrdinalIgnoreCase))?
                                            .GetValue(macroEditorForm);

            if (fieldValue is MacroEditorControl editor)
            {
                return editor.Dirty;
            }
            return false;
        }

        public static void Save(this MacroEditorForm macroEditorForm)
        {
            macroEditorForm
                .GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                .FirstOrDefault(mth => mth.Name.Equals("PerformCommand", StringComparison.OrdinalIgnoreCase))?
                .Invoke(macroEditorForm, new object[] { "Save", null, null, null });
        }

        public static void SetAsDefault(this MacroEditorForm macroEditorForm)
        {
            try
            {
                typeof(Program).GetField("_macroEditor", BindingFlags.Static | BindingFlags.NonPublic)?
                               .SetValue(null, macroEditorForm);
            }
            catch (Exception)
            {
            }
        }
    }
}
