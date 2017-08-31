using SwissAcademic.Citavi.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SwissAcademic.Addons.MacroManager
{
    internal static class Extensions
    {
        #region MacroEditorForm

        public static void Run(this MacroEditorForm macroEditorForm)
        {
            var method = macroEditorForm.GetType()
                                        .GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                                        .FirstOrDefault(mth => mth.Name.Equals("PerformCommand", StringComparison.OrdinalIgnoreCase));

            if (method != null)
            {
                method.Invoke(macroEditorForm, new object[] { "Run", null, null, null });
            }
        }

        public static bool IsHidden(this MacroEditorForm macroEditorForm)
        {
            return macroEditorForm.WindowState == FormWindowState.Minimized && !macroEditorForm.Visible && macroEditorForm.Opacity == 0.00;
        }

        #endregion
    }
}
